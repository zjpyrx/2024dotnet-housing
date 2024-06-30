#include "pch.h"
#include "GeoCalculator.h"
#include <cmath>
#include <iostream>

namespace GeoLibCppCli {
    const double EarthRadiusKm = 6371.0;

    // Define M_PI if not defined
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

    double GeoCalculator::GetDist(double lon1, double lat1, double lon2, double lat2) {
        std::cout << "hello" << std::endl;

        double lat1Rad = M_PI * lat1 / 180.0;
        double lon1Rad = M_PI * lon1 / 180.0;
        double lat2Rad = M_PI * lat2 / 180.0;
        double lon2Rad = M_PI * lon2 / 180.0;

        double dlat = lat2Rad - lat1Rad;
        double dlon = lon2Rad - lon1Rad;
        double a = std::sin(dlat / 2) * std::sin(dlat / 2) +
            std::cos(lat1Rad) * std::cos(lat2Rad) *
            std::sin(dlon / 2) * std::sin(dlon / 2);
        double c = 2 * std::atan2(std::sqrt(a), std::sqrt(1 - a));

        return EarthRadiusKm * c;
    }
}
