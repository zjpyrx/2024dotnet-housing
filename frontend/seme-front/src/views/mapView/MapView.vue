<template>
  <el-container style="font-family: siyuanheiti; color: black">
    <el-header>
    <MyHeader></MyHeader>
  </el-header>
    <el-container>
      <el-aside width="350px">
        <el-input
          class="keywordInput"
          type="text"
          v-model="keyword"
          placeholder="东方明珠"
        />
        <el-button class="searchButton" @click="search" circle
          ><el-icon><Search /></el-icon
        ></el-button>

        <!-- 测试用 -->
        <el-button
          class="detailButton"
          style="display: block"
          @click="showHousingAnalysisViewDetailTest(selectedPoiTest)"
          circle
          ><el-icon><LocationInformation /></el-icon
        ></el-button>

        <!-- 表格 -->
        <table class="addressTable">
          <thead>
            <tr>
              <th style="width: 300px; text-align: center">搜索结果列表</th>
              <th style="width: 30px"></th>
            </tr>
          </thead>
          <tbody>
            <tr
              @click="openInfoWindow(poi)"
              v-for="poi in searchResults"
              :key="poi"
              :class="{
                'result-row': true,
                'result-row-selected': this.selectedRow === poi,
              }"
            >
              <td>
                <el-icon><Location /></el-icon>&nbsp;&nbsp;{{ poi.title }}
              </td>
              <td>
                <el-button
                  class="detailButton"
                  @click="showHousingAnalysisViewDetail(poi)"
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
const BMap = window.BMap;
const { BMAP_STATUS_SUCCESS, BMAP_ANCHOR_TOP_RIGHT, BMAP_ANCHOR_BOTTOM_RIGHT } = window;
import MyHeader from "@/components/MyHeader";

export default {
  data() {
    return {
      map: null,
      popoverVisible: false,
      keyword: localStorage.getItem("keyword"),
      searchResults: [],
      selectedRow: null,
      selectedPoi: {
        title: "东方明珠",
        address: "上海市浦东新区世纪大道1号",
        point: {
          lng: 121.506,
          lat: 31.245,
        },
      },
      selectedPoiTest: {
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
    MyHeader
  },
  mounted() {
    localStorage.setItem("keyword", "东方明珠");
    this.initMap();
    //this.search();
  },
  methods: {
    initMap() {
      this.map = new BMap.Map("mapContainer");
      // 设置地图中心点和缩放级别
      this.map.centerAndZoom(new BMap.Point(121.506, 31.245), 15);
      this.map.enableScrollWheelZoom(true);
      // 添加控件
      this.map.addControl(new BMap.ScaleControl());
      this.map.addControl(
        new BMap.NavigationControl({
          anchor: BMAP_ANCHOR_BOTTOM_RIGHT,
        })
      );
      this.map.addControl(new BMap.CityListControl());
      //this.map.setMapType(BMAP_SATELLITE_MAP);
      this.map.addControl(
        new BMap.MapTypeControl({
          anchor: BMAP_ANCHOR_TOP_RIGHT,
        })
      );
      this.map.setMapStyle({ style: "grassgreen" });
      // 添加地图点击事件监听器
      this.map.addEventListener("click", (e) => {
        const point = e.point;
        const lng = point.lng; // 经度
        const lat = point.lat; // 纬度
        console.log("点击的经度：" + lng);
        console.log("点击的纬度：" + lat);
      });
    },
    showUserInfo() {},
    search() {
      localStorage.setItem("keyword", this.keyword);
      const localSearch = new BMap.LocalSearch(this.map, {
        onSearchComplete: (results) => {
          if (localSearch.getStatus() === BMAP_STATUS_SUCCESS) {
            // 清除地图上的标记
            this.map.clearOverlays();
            this.searchResults = [];
            // 显示搜索结果
            for (let i = 0; i < results.getCurrentNumPois(); i++) {
              const poi = results.getPoi(i);
              this.searchResults.push(poi);
              const marker = new BMap.Marker(poi.point);
              const label = new BMap.Label(String.fromCharCode(65 + i), {
                offset: new BMap.Size(10, -20),
              });
              marker.setLabel(label);
              this.map.addOverlay(marker);

              // 添加点击事件监听器
              marker.addEventListener("click", () => {
                this.openInfoWindow(poi);
              });
            }
            if (results.getNumPois() > 0) {
              this.openInfoWindow(results.getPoi(0));
              this.map.centerAndZoom(results.getPoi(0).point, 15);
            }
          }
        },
      });
      localSearch.setLocation("上海市");
      localSearch.search(this.keyword);
    },
    openInfoWindow(poi) {
      if (this.selectedRow === poi) {
        this.showHousingAnalysisViewDetail();
      } else {
        const infoWindowContent = `
      <div>
        <h4>${poi.title}</h4>
        <p>地址：${poi.address}</p>
        <p>经度：${poi.point.lng}</p>
        <p>纬度：${poi.point.lat}</p>
      </div>
      `;
        const infoWindow = new BMap.InfoWindow(infoWindowContent);
        this.map.openInfoWindow(infoWindow, poi.point);
        this.selectedRow = poi;
        this.selectedPoi = {
          title: poi.title,
          address: poi.address,
          point: {
            lng: poi.point.lng,
            lat: poi.point.lat,
          },
        };
      }
    },
    showHousingAnalysisViewDetail(poi) {
      console.log(JSON.stringify(poi));
      localStorage.setItem("selectedPoi", JSON.stringify(poi));
      this.$router.push("/HousingAnalysisView");
    },
    showHousingAnalysisViewDetailTest(poi) {
      console.log(JSON.stringify(poi));
      localStorage.setItem("selectedPoi", JSON.stringify(poi));
      this.$router.push("/HousingAnalysisView");
    },
  }, // end of methods
}; // end of export
</script>
<style scoped>
.el-header{
  height: 75px;
  min-width: 1300px;
  width:100%;
  padding-left: 0;
  padding-right: 0;
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