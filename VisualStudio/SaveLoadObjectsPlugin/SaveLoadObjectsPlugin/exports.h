#pragma once
#ifdef SAVELOAD_EXPORTS
#define SAVELOAD_API __declspec(dllexport)
#else
#define SAVELOAD_API __declspec(dllimport)
#endif
