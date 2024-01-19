#pragma once

#include "Backend.h"
#include "SFML/Audio.hpp"
#include <json/json.h>
#include "FileManagment.h"

#ifdef BACKEND_EXPORTS
#define BACKEND __declspec(dllexport)
#else
#define BACKEND __declspec(dllimport)
#endif
	class BACKEND AudioManager {
	private:
		sf::SoundBuffer buffer;
		sf::Sound sound;
	public:
		AudioManager() {

		}

		~AudioManager() {

		}

		void playSong(const char* name) {
			Backend::initializeApplication(false);
			Backend::playSong(name, buffer, sound);
		}

		void setVolume(int volume) {
			sound.setVolume(volume);
		}
	};

	class BACKEND ResourceManager {
	private:
		Json::Value loadedDirectory;
		std::vector<Playlist> playlists;
	public:
		void reload() {
			reloadDirectory(loadedDirectory, playlists);
		}
		std::vector<Song> getAvailableSongs() {
			reloadDirectory(loadedDirectory, playlists);
			
		}
	};
	
	extern "C" BACKEND AudioManager * CreateAudioManager() {
		AudioManager* audio = new AudioManager();
		std::cout << "AudioManager created: " << audio << std::endl;
		return audio;
	}

	extern "C" BACKEND ResourceManager* CreateResourceManager() {
		ResourceManager* resource = new ResourceManager();
		DEBUG("Created ResourceManager: " << resource);
		return resource;
	}

	extern "C" BACKEND void ResourceManager_GetAvailableSongs(ResourceManager * resource, Song * *songsArray, int* songCount) {
		std::vector<Song> songs = resource->getAvailableSongs();
		DEBUG("SONG DEBUG: " << songs.at(0).songName);

		*songCount = static_cast<int>(songs.size());
		DEBUG("Song count? is " <<songCount);
		*songsArray = new Song[*songCount];
		DEBUG("Song Array= is " << songsArray);

		for (int i = 0; i < *songCount; i++) {
			(*songsArray)[i] = songs[i];
			DEBUG("SONGNAME IS " << (*songsArray)[i].songName << " before sending");
		}
		DEBUG("FINISHED");
	}

	extern "C" BACKEND void AudioManager_PlaySong(AudioManager* audio, const char* name) {
		audio->setVolume(4);
		std::cout << "Received " << name << std::endl;
		audio->playSong(name);
	}
	extern "C" BACKEND void DestroyAudioManager(AudioManager * audio) {
		std::cout << "Destroying AudioManager: " << audio << std::endl;
		delete audio;
	}
