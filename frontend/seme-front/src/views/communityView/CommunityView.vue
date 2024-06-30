<template>
  <el-container>
    <el-header>
      <MyHeader></MyHeader>
    </el-header>
    <el-container>
      <el-aside width="350px">
        <el-input
          class="keywordInput"
          type="text"
          v-model="keyword"
          placeholder="输入关键词"
        />
        <el-button class="searchButton" @click="searchCommunities" circle
          ><el-icon><Search /></el-icon
        ></el-button>
        <el-button @click="getNearestCommunities">获取附近房源</el-button>

        <table class="addressTable">
          <thead>
            <tr>
              <th>社区名称</th>
              <th>地址</th>
              <th>详情</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="community in communityData"
              :key="community.id"
              @click="openInfoWindow(community)"
              class="result-row"
            >
              <td>{{ community.title }}</td>
              <td>{{ community.address }}</td>
              <td>
                <el-button
                  class="detailButton"
                  @click="jumpToDetail(community)"
                  circle
                  ><el-icon><LocationInformation /></el-icon
                ></el-button>
              </td>
            </tr>
          </tbody>
        </table>
      </el-aside>
      <el-main>        
        <div id="mapContainer" class="mapContainer"></div>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import axios from "axios";
import MyHeader from "@/components/MyHeader";
const { BMAP_SATELLITE_MAP } = window;

const BMap = window.BMap;

export default {
  name: "CommunityView",
  data() {
    return {
      location: null,
      keyword: "",
      latitude: null,
      longitude: null,
      communityData: [],
      myPoint: {
        title: "",
        address: "",
        point: {
          lng: null,
          lat: null,
        },
      },
      map: null,
    };
  },
  components: {
    MyHeader,
  },
  mounted() {
    this.initMap();
  },
  methods: {
    initMap() {
      this.map = new BMap.Map("mapContainer");
      this.map.centerAndZoom(new BMap.Point(121.506, 31.245), 15);
      this.map.enableScrollWheelZoom(true);
      this.map.addControl(new BMap.ScaleControl());
      this.map.addControl(new BMap.NavigationControl());
      this.map.addControl(new BMap.CityListControl());
      this.map.addControl(new BMap.MapTypeControl());
      this.map.setMapStyle({ style: "grassgreen" });
      this.map.setMapType(BMAP_SATELLITE_MAP);
    },
    async searchCommunities() {
      try {
        const response = await axios.get(
          "http://localhost:18101/api/v1/communities/search",
          { params: { keyword: this.keyword } }
        );
        this.communityData = response.data.map((community) => ({
          title: community.communityName,
          address: community.address,
          phoneNumber: community.phoneNumber,
          point: {
            lng: community.longitude,
            lat: community.latitude,
          },
        }));
        this.displayOnMap();
      } catch (error) {
        console.error("Error fetching communities data:", error);
      }
    },
    async getIPAddress() {
      try {
        const response = await axios.get("https://api.ipify.org?format=json");
        return response.data.ip;
      } catch (error) {
        console.error("获取IP地址时出错:", error);
        return null;
      }
    },
    async getNearestCommunities() {
      try {
        const ip = await this.getIPAddress();
        if (ip) {
          const response = await axios.get(
            "http://localhost:18101/api/v1/location/current",
            { params: { ipAddress: ip } }
          );
          this.location = response.data;
        }
        const [latitude, longitude] = this.location.loc.split(",").map(Number);
        this.latitude = latitude;
        this.longitude = longitude;

        const response = await axios.get(
          "http://localhost:18101/api/v1/communities/nearest",
          { params: { latitude: this.latitude, longitude: this.longitude, count: 10 } }
        );
        this.communityData = response.data.map((community) => ({
          title: community.communityName,
          address: community.address,
          phoneNumber: community.phoneNumber,
          point: {
            lng: community.longitude,
            lat: community.latitude,
          },
        }));
        this.displayOnMap();
      } catch (error) {
        console.error("Error fetching nearest communities data:", error);
      }
    },
    getNearestCommunitiesByMap() {
      this.$router.push("/MapView");
    },
    jumpToDetail(poi) {
      console.log(JSON.stringify(poi));
      localStorage.setItem("selectedPoi", JSON.stringify(poi));
      console.log("jp");
      this.$router.push("/HousingAnalysisView");
    },
    displayOnMap() {
      this.map.clearOverlays();
      this.communityData.forEach((poi) => {
        console.log(1)
        const point = new BMap.Point(poi.point.lng, poi.point.lat);
        const marker = new BMap.Marker(point);
        this.map.addOverlay(marker);
      
        marker.addEventListener("click", () => {
          this.openInfoWindow(poi);
        });
      });
      const curpoint = new BMap.Point(this.communityData[0].point.lng, this.communityData[0].point.lat);
      this.map.centerAndZoom(curpoint, 15);
      this.openInfoWindow(this.communityData[0]);
    },
    openInfoWindow(poi) {
      console.log(poi.title)
      console.log(poi.address)
      console.log(poi.point.lng)
      console.log(poi.point.lat)

        const infoWindowContent = `
      <div>
        <h4>${poi.title}</h4>
        <p>地址：${poi.address}</p>
        <p>经度：${poi.point.lng}</p>
        <p>纬度：${poi.point.lat}</p>
      </div>
      `;
        const infoWindow = new BMap.InfoWindow(infoWindowContent);
        const curpoint = new BMap.Point(poi.point.lng, poi.point.lat);
        this.map.openInfoWindow(infoWindow, curpoint); 
    },

  },
};
</script>
  
  
  <style scoped>
.el-header {
  height: 75px;
  min-width: 1300px;
  width: 100%;
  padding-left: 0;
  padding-right: 0;
}
.el-aside {
  background-color: #e3ddcf;
  background-image: url("@/assets/background.png");
  background-size: aotu 100%;
}

.el-main {
  min-width: 1300px;
  height: 1000px;
  background-color: #f4f1eb;
  background-image: url("@/assets/background.png");
  background-size: 100% auto;
}
.pointSearchCard {
  background-image: url("@/assets/pointSearch.jpg");
  color: #000000;
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: zcool-qingke-jianheiti;
  font-size: 60px;
  background-size: cover;
  margin-top: 5px;
  height: 100%;
  width: 100%;
  transition: transform 0.3s ease, background-image 0.3s;
}
.mapContainer {
  margin-top: -20px;
  height: 90vh;
  width: 100%;
}
.el-aside {
  background-color: #e3ddcf;
  background-image: url("@/assets/background.png");
  background-size: aotu 100%;
}

.el-main {
  min-width: 800px;
  background-color: #f4f1eb;
}

.keywordInput {
  margin-top: 10px;
  width: 250px;
  left: 0;
}
.searchButton {
  margin-top: 10px;
  margin-left: 5px;
  color: #ffffff;
  background-color: #000000;
  transition: background-color 0.3s;
}
.searchButton:hover {
  background-color: #56a86b;
  color: rgb(0, 0, 0);
}
.detailButton {
  color: white;
  background: black;
  transition: background-color 0.3s;
}
.detailButton:hover {
  background-color: #56a86b;
  color: rgb(0, 0, 0);
}
.addressTable {
  text-align: left;
}
.result-row {
  transition: background-color 0.3s;
}
.result-row-selected {
  color: #ffffff;
  background-color: #000000;
}
.result-row:hover {
  cursor: pointer;
  color: #000000;
  background-color: #d8c9a5;
}
</style>
  
  
  