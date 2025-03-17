#include "pch.h"
#include "mydll.h"

HANDLE hMapMutex = CreateMutex(NULL, FALSE, L"MapMutex");

void MapSendMessage(int addr, const wchar_t* str)
{
    WaitForSingleObject(hMapMutex, INFINITE);

    HANDLE hFile = CreateFile(L"filemap.dat", GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_ALWAYS, 0, 0);

    header h = { addr, (int)(wcslen(str) * sizeof(wchar_t)) };

    HANDLE hFileMap = CreateFileMapping(hFile, NULL, PAGE_READWRITE, 0, sizeof(header) + h.size, L"MyMap");
    BYTE* buff = (BYTE*)MapViewOfFile(hFileMap, FILE_MAP_ALL_ACCESS, 0, 0, sizeof(header) + h.size);

    memcpy(buff, &h, sizeof(header));
    memcpy(buff + sizeof(header), str, h.size);

    UnmapViewOfFile(buff);
    CloseHandle(hFileMap);
    CloseHandle(hFile);

    ReleaseMutex(hMapMutex);
}

std::wstring MapReceiveMessage(header& h)
{
    WaitForSingleObject(hMapMutex, INFINITE);

    HANDLE hFile = CreateFile(L"filemap.dat", GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, 0);

    HANDLE hFileMap = CreateFileMapping(hFile, NULL, PAGE_READWRITE, 0, sizeof(header), L"MyMap");
    LPVOID buff = MapViewOfFile(hFileMap, FILE_MAP_ALL_ACCESS, 0, 0, sizeof(header));

    h = *((header*)buff);

    UnmapViewOfFile(buff);
    CloseHandle(hFileMap);

    int n = h.size + sizeof(header);
    hFileMap = CreateFileMapping(hFile, NULL, PAGE_READWRITE, 0, n, L"MyMap");
    buff = MapViewOfFile(hFileMap, FILE_MAP_ALL_ACCESS, 0, 0, n);

    std::wstring str((wchar_t*)((BYTE*)buff + sizeof(header)), h.size / sizeof(wchar_t));

    UnmapViewOfFile(buff);
    CloseHandle(hFileMap);
    CloseHandle(hFile);

    ReleaseMutex(hMapMutex);

    return str;
}