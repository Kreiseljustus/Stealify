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
		static char* statusMessage;
		static int currentProgress;

		Json::Value getDirectory() {
			DEBUG(loadedDirectory);
			return loadedDirectory;
		}
		void reload() {
			reloadDirectory(loadedDirectory, playlists);
		}
	};

	char* ResourceManager::statusMessage = nullptr;
	int ResourceManager::currentProgress = 0;
	
#pragma region ExternAudioManager
	extern "C" BACKEND AudioManager * CreateAudioManager() {
		AudioManager* audio = new AudioManager();
		std::cout << "AudioManager created: " << audio << std::endl;
		return audio;
	}
	extern "C" BACKEND void AudioManager_PlaySong(AudioManager * audio, const char* name) {
		audio->setVolume(4);
		std::cout << "Received " << name << std::endl;
		audio->playSong(name);
	}
	extern "C" BACKEND void DestroyAudioManager(AudioManager * audio) {
		std::cout << "Destroying AudioManager: " << audio << std::endl;
		delete audio;
	}
#pragma endregion

#pragma region ExternResourceManager

	extern "C" BACKEND int GetCurrentProgress(ResourceManager* resource) {
		return (resource->currentProgress);
	}

	extern "C" BACKEND const char* GetCurrentStatusMessage(ResourceManager * resource) {
		return (resource->statusMessage);
	}

	extern "C" BACKEND ResourceManager* CreateResourceManager() {
		ResourceManager* resource = new ResourceManager();
		DEBUG("Created ResourceManager: " << resource);
		return resource;
	}

	extern "C" BACKEND const char* ResourceManager_GetAvailableSongs(ResourceManager * resource) {
		DEBUG("BEFORE RELOAD");
		resource->reload();
		Json::StreamWriterBuilder writerBuilder;
		std::string jsonString = Json::writeString(writerBuilder, resource->getDirectory()["songs"]);
		return _strdup(jsonString.c_str());
	}

	extern "C" BACKEND void DestroyResourceManager(ResourceManager * resource) {
		DEBUG("Destroying AudioManager: " << resource);
		delete resource;
	}
#pragma endregion