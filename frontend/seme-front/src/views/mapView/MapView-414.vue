<template>
    <input type="text" v-model="keyword" placeholder="输入关键字" />
    <input type="text" v-model="location" placeholder="输入位置" />
    <button @click="search">搜索0</button>
    <input
      type="text"
      v-model="searchInput"
      @input="searchPlaces"
      placeholder="输入名称搜索"
    />
    <select v-model="selectedPlace" @change="changePlace">
      <option v-for="place in this.searchResults" :key="place" :value="place">
        {{ place.title }}
      </option>
    </select>
    <el-button @click="searchPos">搜索</el-button>
    <div>
      <!-- 表格 -->
      <table>
        <thead>
          <tr>
            <th>地名</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="poi in searchResults" :key="poi">
            <td>{{ poi.title }}</td>
            <td>
              <button @click="openInfoWindow(poi)">打开</button>
            </td>
          </tr>
        </tbody>
      </table>
  
      <!-- 地点信息框 -->
      <div>
        <h3>地点信息</h3>
        <p>地址: {{ selectedPoi.address }}</p>
        <p>经度: {{ selectedPoi.point.lng }}</p>
        <p>纬度: {{ selectedPoi.point.lat }}</p>
      </div>
    </div>
    <div id="mapContainer" class="mapContainer"></div>
  </template>
  
  <script>
  const BMap = window.BMap;
  //const { BMAP_SATELLITE_MAP } = window;
  const { BMAP_STATUS_SUCCESS } = window;
  export default {
    data() {
      return {
        map: null,
        addressInfo: {
          longitude: "1", // 经度
          latitude: "1", // 纬度
          province: "1", // 省
          city: "1", // 市
          district: "1", // 区
          address: "1", // 详细地址
        },
        searchInput: "",
        selectedPlace: null,
        keyword: "",
        location: "",
        searchResults: [],
        selectedPoi: null,
      };
    },
    mounted() {
      this.initMap();
    },
    methods: {
      initMap() {
        this.map = new BMap.Map("mapContainer");
        // 设置地图中心点和缩放级别
        this.map.centerAndZoom(new BMap.Point(116.404, 39.915), 15);
        this.map.enableScrollWheelZoom(true);
        // 添加控件
        this.map.addControl(new BMap.ScaleControl());
        this.map.addControl(new BMap.NavigationControl());
        this.map.addControl(new BMap.CityListControl());
        //this.map.setMapType(BMAP_SATELLITE_MAP);
  
        // 添加地图点击事件监听器
        this.map.addEventListener("click", (e) => {
          const point = e.point;
          const lng = point.lng; // 经度
          const lat = point.lat; // 纬度
          console.log("点击的经度：" + lng);
          console.log("点击的纬度：" + lat);
        });
      },
      search() {
        const localSearch = new BMap.LocalSearch(this.map, {
          onSearchComplete: (results) => {
            if (localSearch.getStatus() === BMAP_STATUS_SUCCESS) {
              // 清除地图上的标记
              this.map.clearOverlays();
              // 显示搜索结果
              for (let i = 0; i < results.getCurrentNumPois(); i++) {
                const poi = results.getPoi(i);
                const marker = new BMap.Marker(poi.point);
                const label = new BMap.Label(String.fromCharCode(65 + i), {
                  offset: new BMap.Size(10, -20),
                });
                marker.setLabel(label);
                this.map.addOverlay(marker);
  
                // 添加点击事件监听器
                marker.addEventListener("click", () => {
                  const infoWindow = new BMap.InfoWindow(`
                <div>
                  <h4>${poi.title}</h4>
                  <p>地址：${poi.address}</p>
                  <p>经度：${poi.point.lng}</p>
                  <p>纬度：${poi.point.lat}</p>
                </div>
              `);
                  this.map.openInfoWindow(infoWindow, poi.point);
                  //this.map.centerAndZoom(poi.point, 15);
                });
              }
              if (results.getNumPois() > 0) {
                this.map.centerAndZoom(results.getPoi(0).point, 15);
              }
            }
          },
        });
        localSearch.search(this.keyword, { location: this.location });
      },
      openInfoWindow(poi) {
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
  
      // 更新选定的地点信息
      this.selectedPoi = {
        address: poi.address,
        point: {
          lng: poi.point.lng,
          lat: poi.point.lat
        }
      };
    },
      searchPos() {
        var ls = new BMap.LocalSearch(this.map, {
          renderOptions: { map: this.map },
        });
        ls.search("天安门");
        ls.setSearchCompleteCallback((results) => {
          // 获取搜索结果数量
          var count = results.getNumPois();
          console.log("搜索结果数量：" + count);
          if (count > 3) {
            count = 3;
          }
          // 遍历搜索结果
          for (var i = 0; i < count; i++) {
            // 获取每个搜索结果的信息
            let poi = results.getPoi(i);
            console.log("地点名称：" + poi.title);
            console.log("地点地址：" + poi.address);
            console.log("地点经度：" + poi.point.lng);
            console.log("地点纬度：" + poi.point.lat);
            console.log("------------------------");
  
            var point = poi.point;
            var marker = new BMap.Marker(point);
            this.map.addOverlay(marker); // 添加标注
  
            // 创建信息窗口对象
            var infoWindow = new BMap.InfoWindow(poi.title, {
              width: 200,
              height: 100,
            });
            // 绑定信息窗口到标注上
            marker.addEventListener("click", function () {
              this.openInfoWindow(infoWindow);
            });
          }
        });
      },
      searchPlaces() {
        if (this.searchInput) {
          var localSearch = new BMap.LocalSearch(this.map, {
            onSearchComplete: this.onSearchComplete,
          });
          localSearch.search(this.searchInput);
        } else {
          this.searchResults = [];
        }
      },
      onSearchComplete(results) {
        //this.searchResults = results.getPoi(0);
        this.searchResults = [];
        var count = results.getNumPois();
        console.log("搜索结果数量：" + count);
        if (count > 10) {
          count = 10;
        }
        for (var i = 0; i < count; i++) {
          // 获取每个搜索结果的信息
          var poi = results.getPoi(i);
          this.searchResults.push(poi);
          console.log("地点名称：" + poi.title);
          console.log("地点地址：" + poi.address);
          console.log("地点经度：" + poi.point.lng);
          console.log("地点纬度：" + poi.point.lat);
          console.log("------------------------");
        }
      },
      changePlace() {
        var infoWindow = new BMap.InfoWindow("");
        // 更新气泡内容和显示位置
        infoWindow.setContent(this.selectedPlace.title);
        var point = new BMap.Point(
          this.selectedPlace.lng,
          this.selectedPlace.lat
        );
        this.map.openInfoWindow(infoWindow, point);
        if (this.selectedPlace) {
          this.map.centerAndZoom(this.selectedPlace.point, 15);
        }
      },
    }, // end of methods
  }; // end of export
  </script>
  <style scoped>
  .mapContainer {
    height: 90vh;
    width: 90%;
  }
  </style>