using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Core
{
    class BackendAPI
    {
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateAudioManager();
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateResourceManager();

        public static AudioManager createAudioManager()
        {
            IntPtr audioManager = CreateAudioManager();
            return new AudioManager(audioManager);
        }

        public static ResourceManager createResourceManager()
        {
            IntPtr resourceManager = CreateResourceManager();
            return new ResourceManager(resourceManager);
        }
    }

    class AudioManager
    {
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyAudioManager(IntPtr audio);
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void AudioManager_PlaySong(IntPtr audio, [MarshalAs(UnmanagedType.LPStr)] string name);

        private IntPtr pointer;

        public void playSong(string songName)
        {
            AudioManager_PlaySong(pointer, songName);
        }

        //Obtain the required pointer from the CreateAudioManager method exposed by the Backend.dll
        public AudioManager(IntPtr audioManagerPointer) { 
            pointer = audioManagerPointer; 
        }

        ~AudioManager()
        {
            DestroyAudioManager(pointer);
        }
    }

    class ResourceManager
    {
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ResourceManager_GetAvailableSongs(IntPtr resourceManager, out IntPtr songsArray, out int songCount);

        private IntPtr pointer;

        public ResourceManager(IntPtr resourceManagerPointer)
        {
            pointer = resourceManagerPointer;
        }

        private static List<Song> IntPtrArrayToList(IntPtr songsArray, int songCount)
        {
            List<Song> list = new List<Song>();
            int structSize = Marshal.SizeOf<Song>();

            // Create a managed array to hold the data
            byte[] dataArray = new byte[structSize * songCount];

            // Copy the data from native array to managed array
            Marshal.Copy(songsArray, dataArray, 0, dataArray.Length);

            // Create a GCHandle to pin the managed array in memory
            GCHandle handle = GCHandle.Alloc(dataArray, GCHandleType.Pinned);
            Console.WriteLine("Data: " + BitConverter.ToString(dataArray));
            try
            {
                // Use PtrToStructure to convert the pinned data to a Song structure
                for (int i = 0; i < songCount; i++)
                {
                    IntPtr currentPtr = IntPtr.Add(handle.AddrOfPinnedObject(), i * structSize);
                    Song currentSong = Marshal.PtrToStructure<Song>(currentPtr);

                    // Access string fields directly, no need for PtrToStringAnsi
                    Console.WriteLine(currentSong.SongName + " this is current songname");
                    list.Add(currentSong);
                }
            }
            finally
            {
                // Release the pinned memory
                handle.Free();
            }

            return list;
        }




        public List<Song> getSongs() {
            ResourceManager_GetAvailableSongs(pointer, out IntPtr songsArray, out int songCount);
            Console.WriteLine("Before converting: received 0x" + songsArray.ToString("X") + " as array pointer");
            Console.WriteLine("Before converting: " + "received " + songCount + " as song count");
            List<Song> availableSongs = IntPtrArrayToList(songsArray, songCount);
            return availableSongs;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Song
    {
        // Point to mp3 or other type of file (.wav, .ogg)
            [MarshalAs(UnmanagedType.LPStr)]
            public string StorageLocation;
            public int SizeInBytes;

            [MarshalAs(UnmanagedType.LPStr)]
            public string SongName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Artist;

            public bool PartiallyLoaded;
            public bool IsPlaying;
            public bool IsPaused;

            // TODO
            public bool HasLyricsAvailable;

            public int TimeRemaining;
    }

}
