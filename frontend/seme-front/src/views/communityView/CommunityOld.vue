<template>
    <el-container>
      <el-header>
        <MyHeader></MyHeader>
      </el-header>
      <el-container>
        <el-main>
          <div>
            <el-input v-model="keyword" placeholder="输入关键词"></el-input>
            <el-button @click="searchCommunities">搜索房源</el-button>
          </div>
          <div>
            <el-button @click="getNearestCommunities">获取附近房源</el-button>
            <el-button @click="getNearestCommunitiesByMap">通过地图选点获取附近房源</el-button>
          </div>
          <div v-if="location">
          <p>IP: {{ location.ip }}</p>
          <p>城市: {{ location.city }}</p>
          <p>区域: {{ location.region }}</p>
          <p>国家: {{ location.country }}</p>
          <p>位置: {{ location.loc }}</p>
        </div>
          <table>
            <thead>
              <tr>
                <th>社区名称</th>
                <th>地址</th>
                <th>电话号码</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="community in communityData"
                :key="community.id"
                @click="jumpToDetail(community)"
              >
                <td>{{ community.communityName }}</td>
                <td>{{ community.address }}</td>
                <td>{{ community.phoneNumber }}</td>
              </tr>
            </tbody>
          </table>
        </el-main>
      </el-container>
    </el-container>
  </template>
    
    <script>
  import axios from "axios";
  import MyHeader from "@/components/MyHeader";
  
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
          title: "东方明珠塔测试用",
          address: "上海市浦东新区世纪大道1号",
          point: {
            lng: 121.506,
            lat: 31.245,
          },
        },
      };
    },
    components: {
      MyHeader,
    },
    methods: {
      async searchCommunities() {
        try {
          const response = await axios.get(
            "http://localhost:18101/api/v1/communities/search",
            {
              params: { keyword: this.keyword },
            }
          );
          this.communityData = response.data;
        } catch (error) {
          console.error("Error fetching communities data:", error);
        }
      },
      async getIPAddress() {
          try {
            const response = await axios.get('https://api.ipify.org?format=json');
            return response.data.ip;
          } catch (error) {
            console.error('获取IP地址时出错:', error);
            return null;
          }
        },
      async getNearestCommunities() {
        try {
            const ip = await this.getIPAddress();
            if (ip) {
              const response = await axios.get('http://localhost:18101/api/v1/location/current', {
                params: { ipAddress: ip },
              });
              this.location = response.data;
            }
          } catch (error) {
            console.error('获取位置数据时出错:', error);
          }
          const [latitude, longitude] = this.location.loc.split(',').map(Number);
          this.latitude = latitude;
          this.longitude = longitude;
          console.log(this.latitude)
          console.log(this.longitude)
        try {
          const response = await axios.get(
            "http://localhost:18101/api/v1/communities/nearest",
            {
              params: {
                latitude: this.latitude,
                longitude: this.longitude,
                count: 10,
              },
            }
          );
          this.communityData = response.data;
        } catch (error) {
          console.error("Error fetching nearest communities data:", error);
        }
      },
      getNearestCommunitiesByMap(){
        this.$router.push('/MapView');
      },
      jumpToDetail(poi) {
        this.myPoint.title = poi.communityName;
        this.myPoint.address = poi.address;
        this.myPoint.point.lng = poi.longitude;
        this.myPoint.point.lat = poi.latitude;
        console.log(JSON.stringify(this.myPoint));
        localStorage.setItem("selectedPoi", JSON.stringify(this.myPoint));
        this.$router.push("/HousingAnalysisView");
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
  </style>
    
    
    