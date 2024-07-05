#pragma once

using namespace System;

namespace GeoLibCppCli {
    public ref class GeoCalculator {
    public:
        static double GetDist(double lon1, double lat1, double lon2, double lat2);
    };
}
