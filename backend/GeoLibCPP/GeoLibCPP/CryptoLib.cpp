#include <iostream>
#include <cstring>
#include <vector>
#include <algorithm>

#define AES_BLOCK_SIZE 16

// Simple XOR encryption for demonstration (not secure for real use)
void XOREncrypt(const char* input, char* output, const char* key, int length) {
    for (int i = 0; i < length; ++i) {
        output[i] = input[i] ^ key[i % AES_BLOCK_SIZE];
    }
}

extern "C" __declspec(dllexport) void EncryptAES(const char* input, char* output, const char* key) {
    int length = strlen(input);
    int padding = AES_BLOCK_SIZE - length % AES_BLOCK_SIZE;
    std::vector<char> paddedInput(length + padding);
    memcpy(paddedInput.data(), input, length);
    memset(paddedInput.data() + length, padding, padding);

    XOREncrypt(paddedInput.data(), output, key, length + padding);
}

extern "C" __declspec(dllexport) void DecryptAES(const char* input, char* output, const char* key) {
    int length = strlen(input);
    XOREncrypt(input, output, key, length);

    // Remove padding
    int padding = output[length - 1];
    output[length - padding] = '\0';
}
