#include "ExportManager.h"

std::string ResourceManager::statusMessage = "None";
int ResourceManager::currentProgress = 0;

AudioManager::AudioManager() {
}

AudioManager::~AudioManager() {
}

void AudioManager::playSong(const char* name) {
    Backend::initializeApplication(false);
    Backend::playSong(name, buffer, sound);
}

void AudioManager::setVolume(int volume) {
    sound.setVolume(volume);
}

Json::Value ResourceManager::getDirectory() {
    DEBUG(loadedDirectory);
    return loadedDirectory;
}

void ResourceManager::downloadSong(std::string url, std::string songName, std::string artist) {
    DEBUG("CALLING BACKEND");
    DEBUG("2: " << url);
    Backend::addSong(url, songName, artist);
}

void ResourceManager::reload() {
    reloadDirectory(loadedDirectory, playlists);
}

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

extern "C" BACKEND void DownloadSong(ResourceManager * pointer, const char* songUrl, const char* songName, const char* artist) {
    DEBUG("CALLED");
    DEBUG("URL IS " << songUrl);
    DEBUG("NAME IS " << songName);
    DEBUG("ARTIST IS " << artist);
    pointer->downloadSong(songUrl, songName, artist);
}

extern "C" BACKEND int GetCurrentProgress(ResourceManager * resource) {
    return (resource->currentProgress);
}

extern "C" BACKEND const char* GetCurrentStatusMessage(ResourceManager * resource) {
    return (resource->statusMessage.c_str());
}

extern "C" BACKEND ResourceManager * CreateResourceManager() {
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