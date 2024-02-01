#pragma once
#ifdef NDEBUG
	#define DEBUG(str) do {} while(0);
#else
	#define DEBUG(str) do { std::cout << "Backend: "<< str << std::endl; } while(0);
#endif