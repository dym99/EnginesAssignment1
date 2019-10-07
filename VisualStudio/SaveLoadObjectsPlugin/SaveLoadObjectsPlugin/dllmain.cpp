// dllmain.cpp : Defines the entry point for the DLL application.
#define _CRT_SECURE_NO_WARNINGS
#include <Windows.h>
#include <vector>
#include <fstream>
#include <stdio.h>
#include <stdlib.h>
#include "vec.h"
#include "exports.h"


void exportObjects(const char* fileName, std::vector<objectInfo> ob)
{
	FILE* file = fopen(fileName, "w");

	if (file == NULL) {
		//The file couldn't be opened
		return;
	}
	for (int i = 0; i < ob.size(); ++i)
	{
		objectInfo info = ob[i];
		fprintf(file, "%i %f %f %f %f %f %f %f\n", info.id, info.pos.x, info.pos.y, info.pos.z, info.rot.x, info.rot.y, info.rot.z, info.rot.w);
	}

	fclose(file);
}

void importObjects(const char* fileName, std::vector<objectInfo>& ob) {
	FILE* file = fopen(fileName, "r");

	if (file == NULL) {
		//The file couldn't be opened
		return;
	}

	while (!feof(file))
	{
		objectInfo info;
		fscanf(file, "%i %f %f %f %f %f %f %f", &info.id, &info.pos.x, &info.pos.y, &info.pos.z, &info.rot.x, &info.rot.y, &info.rot.z, &info.rot.w);
		ob.push_back(info);
	}

	fclose(file);
}


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}


extern "C" {
	SAVELOAD_API void saveToFile(const char* filename, int obCount, objectInfo obs[]) {
		std::vector<objectInfo> obVector = std::vector<objectInfo>(obCount);
		for (int i = 0; i < obCount; ++i) {
			obVector[i]=obs[i];
		}
		exportObjects(filename, obVector);
	}

	SAVELOAD_API objectInfo* loadFromFile(const char* filename, int& obCount) {
		std::vector<objectInfo> infoVec = std::vector<objectInfo>();
		importObjects(filename, infoVec);
		obCount = infoVec.size();
		return &infoVec[0];
	}
}