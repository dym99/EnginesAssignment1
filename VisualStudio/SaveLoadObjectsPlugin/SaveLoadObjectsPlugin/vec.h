#pragma once


typedef struct vec3
{
	float x, y, z;
} vec3;

typedef struct quat
{
	float x, y, z, w;
} quat;

typedef struct objectInfo
{
	int id; vec3 pos; quat rot;
} objectInfo;