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

#define RMD_FAIL() do {ResourceManager::statusMessage = "Failed!";} while(0);

class BACKEND AudioManager {
private:
    sf::SoundBuffer buffer;
    sf::Sound sound;

public:
    AudioManager();
    ~AudioManager();
    void playSong(const char* name);
    void setVolume(int volume);
};

class BACKEND ResourceManager {
private:
    Json::Value loadedDirectory;
    std::vector<Playlist> playlists;

public:
    static std::string statusMessage;
    static int currentProgress;

    void downloadSong(std::string url, std::string songName, std::string artist);
    Json::Value getDirectory();
    void reload();
};

