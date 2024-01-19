#pragma once
#include <vector>
#include <string>

#pragma pack(push, 1)
struct Song {
	//Point to mp3 or other type of file (.wav, .ogg)
	std::string storageLocation;
	int sizeInBytes;

	std::string songName;
	std::string artist;

	bool partiallyLoaded = false;
	bool isPlaying = false;
	bool isPaused = false;

	//TODO
	bool hasLyricsAvailable = false;

	int timeRemaining;
};
#pragma pack(pop)

struct Playlist {
	std::vector<std::string> songs;

	std::string name;
	std::string creationDate;

	int songIndex = 0;

	bool isPlaying = false;
	bool isPaused = false;
};