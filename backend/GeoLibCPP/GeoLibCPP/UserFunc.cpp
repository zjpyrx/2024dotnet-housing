#include"UserFunc.h"
#include<stdio.h>
#include<iostream>

int add(int a, int b) {
	printf("hello python call dll\n");
	std::cout << "hello world" << std::endl;
	return a + b;
}