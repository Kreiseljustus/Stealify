using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Core
{
    class BackendAPI
    {

        private static BackendAPI instance;
        public static BackendAPI getBackendAPI()
        {
            if(instance == null)
            {
                instance = new BackendAPI();
            }
            return instance;
        }

        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateAudioManager();
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateResourceManager();

        public AudioManager createAudioManager()
        {
            IntPtr audioManager = CreateAudioManager();
            return new AudioManager(audioManager);
        }

        public ResourceManager createResourceManager()
        {
            IntPtr resourceManager = CreateResourceManager();
            return new ResourceManager(resourceManager);
        }
    }

    class AudioManager
    {
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr CreateAudioManager();

        private static AudioManager instance;
        public static AudioManager getAudioManager()
        {
            if(instance == null)
            {
                IntPtr p = CreateAudioManager();
                instance = new AudioManager(p);
            }
            return instance;
        }

        readonly IntPtr _pointer;

        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DestroyAudioManager(IntPtr audio);
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void AudioManager_PlaySong(IntPtr audio, [MarshalAs(UnmanagedType.LPStr)] string name);

        public void playSong(string songName)
        {
            AudioManager_PlaySong(_pointer, songName);
        }

        //Obtain the required pointer from the CreateAudioManager method exposed by the Backend.dll
        public AudioManager(IntPtr audioManagerPointer) { 
            _pointer = audioManagerPointer; 
        }

        ~AudioManager()
        {
            DestroyAudioManager(_pointer);
        }
    }

    class ResourceManager
    {
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern string GetCurrentStatusMessage(IntPtr resource);
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetCurrentProgress(IntPtr resource);
        [DllImport("Backend.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ResourceManager_GetAvailableSongs(IntPtr resource);

        [DllImport("Backend.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern void DestroyResourceManager(IntPtr resource);

        private IntPtr pointer;

        public ResourceManager(IntPtr resourceManagerPointer)
        {
            pointer = resourceManagerPointer;
        }
        ~ResourceManager()
        {
            DestroyResourceManager(pointer);
        }

        public List<Song> getSongs() {
            string result = Marshal.PtrToStringAnsi(ResourceManager_GetAvailableSongs(pointer));

            Dictionary<string, Song> Songs = new Dictionary<string, Song>();
            Songs = JsonConvert.DeserializeObject<Dictionary<string, Song>>(result);

            if (Songs == null) { return new List<Song> { new Song { SongName = "Head over to the download section" } }; }
            
            List<Song> songsList = Songs.Values.ToList();

            return songsList;
        }

        public int getCurrentProgress()
        {
            return GetCurrentProgress(pointer);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Song
    {


        [JsonProperty("storageLocation")]
        public string StorageLocation { get; set; }

        [JsonProperty("sizeInBytes")]
        public int SizeInBytes { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("songName")]
        public string SongName { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("partiallyLoaded")]
        public bool PartiallyLoaded { get; set; }

        [JsonProperty("isPlaying")]
        public bool IsPlaying { get; set; }

        [JsonProperty("isPaused")]
        public bool IsPaused { get; set; }

        [JsonProperty("hasLyricsAvailable")]
        public bool HasLyricsAvailable { get; set; }

        [JsonProperty("timeRemaining")]
        public int TimeRemaining { get; set; }

        [JsonIgnore]
        public string FormattedDuration
        {
            get
            {
                int minutes = Duration / 60;
                int seconds = Duration % 60;

                return $"{minutes:D2}:{seconds:D2}";
            }
        }
    }
}
