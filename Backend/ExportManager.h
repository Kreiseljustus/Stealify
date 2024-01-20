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
		Json::Value getDirectory() {
			return loadedDirectory;
		}
		void reload() {
			reloadDirectory(loadedDirectory, playlists);
		}
		std::vector<Song> getAvailableSongs() {
			reloadDirectory(loadedDirectory, playlists);
			Json::Value songsArray = loadedDirectory["songs"];

			std::vector<Song> songStructArray;
			// Check if "songs" is an object
			if (songsArray.isObject()) {
				// Iterate over the keys (song names) in the "songs" object
				for (const auto& songName : songsArray.getMemberNames()) {
					Song song;

					// Access individual songs using songName
					Json::Value songJson = songsArray[songName];

					// Assign values to the Song struct
					song.storageLocation = songJson["storageLocation"].asString();
					song.sizeInBytes = songJson["sizeInBytes"].asInt();
					song.songName = songJson["songName"].asString();
					song.artist = songJson["artist"].asString();
					song.partiallyLoaded = songJson["partiallyLoaded"].asBool();
					song.isPlaying = songJson["isPlaying"].asBool();
					song.isPaused = songJson["isPaused"].asBool();
					song.hasLyricsAvailable = songJson["hasLyricsAvailable"].asBool();
					song.timeRemaining = songJson["timeRemaining"].asInt();
					DEBUG(song.songName << " in loop");
					// Add the song struct to the vector
					songStructArray.push_back(song);
				}
			}
			return songStructArray;
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

	extern "C" BACKEND const char* ResourceManager_GetAvailableSongs(ResourceManager * resource) {
		DEBUG("BEFORE RELOAD");
		resource->reload();
		Json::StreamWriterBuilder writerBuilder;
		std::string jsonString = Json::writeString(writerBuilder, resource->getDirectory()["songs"]);
		return _strdup(jsonString.c_str());
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
