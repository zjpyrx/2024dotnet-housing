<template>
  <el-container style="font-family: siyuanheiti; color: black">
    <el-header>
      <MyHeader></MyHeader>
    </el-header>
    <el-container>
      <el-aside width="500px">
        <!---------------------------------没解决的问题：点击marker打不开InfoWindow------------------------------------------->
        <!--    修改1     -->
        <el-card
          style="
            background: #000000;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.4);
            position: relative; /* 确保图片能相对于此元素定位 */
            width: 100%; /* 假设你设置了一个宽度，或者让它自适应 */
            height:280px;
          "
        >
          <!--<div class="streetImg" @click="showImg" :style="streetviewStyle"></div>-->

          <img class="top-image" :src="imageSrc" alt="Street View" />
      </el-card>
      <el-card
          style="
            background: #000000;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.4);
            position: relative; /* 确保图片能相对于此元素定位 */
            width: 100%; /* 假设你设置了一个宽度，或者让它自适应 */
            height:280px;
          "
          v-if="showBottomImg"
        >
          <img
            class="bottom-image"
            :src="resultSrc"
            alt="Result View"
            
          />
      </el-card>
      <el-button @click="showBottomImgCard" class="imgButton">{{ BottomImgText }}</el-button>
        <!--    修改1结束     -->
        <div
          style="
            margin-top: 20px;
            display: flex;
            flex-direction: row;
            color: black;
          "
        >
          <div style="width: 45%">
            <el-button class="LikeButton" @click="addFavorite" circle
              ><el-icon><Star /></el-icon
            ></el-button>
            收藏地点-
          </div>
          <div style="width: 45%">
            <el-button class="NoteButton" @click="addRemark" circle
              ><el-icon><Edit /></el-icon
            ></el-button>
            备注地点-
          </div>
        </div>

        <div class="pointTitle">◤{{ selectedPoi.title }} -</div>
        <div class="infoBox">
          <!-- 地点信息框 -->
          <div class="detailInfo">
            <div class="subtitleBox">&nbsp;&nbsp;- 地点详细信息 -</div>
            <div style="margin-top: 15px">■地址: {{ selectedPoi.address }}</div>
            <div>■经度: {{ selectedPoi.point.lng }}</div>
            <div>■纬度: {{ selectedPoi.point.lat }}</div>
            <div>■街道名称：{{ this.streetViewInfo.roadName }}</div>
            <div>■绿视率：{{ this.streetViewInfo.vegetationPortion }}</div>
            <div>■天空占比：{{ this.streetViewInfo.skyPortion }}</div>
          </div>
          <!-- 周边信息框 -->
          <div class="facilityInfo">
            <div
              class="subtitleBox"
              style="background: #56a86b; color: #000000"
            >
              &nbsp;&nbsp;- 周边设施 -
            </div>
            <svg
              style="margin-top: 20px"
              :width="radius * 2.1"
              :height="radius * 2.1"
            >
              <circle :cx="radius" :cy="radius" :r="radius" class="circle" />
              <circle
                v-for="(facility, index) in facilities"
                :key="index"
                :cx="radius"
                :cy="radius"
                :r="calculateDistance(facility.distance)"
                class="facility"
                :fill="facility.color"
              />
              <text
                v-for="(facility, index) in facilities"
                :key="index"
                :x="radius"
                :y="radius + calculateDistance(facility.distance) - 8"
                class="name"
              >
                {{ facility.name }}
              </text>
            </svg>
            <div
              class="facilityDistance"
              v-for="(facility, index) in facilities"
              :key="index"
            >
              <div>
                <div
                  :style="{ color: facility.color, display: 'inline-block' }"
                >
                  ■
                </div>
                {{ facility.name }}: {{ formatDistance(facility.distance) }}km
              </div>
            </div>
          </div>
        </div>
      </el-aside>
      <el-main>
        <div id="mapContainer" class="mapContainer"></div>
        <div style="display: flex; align-items: flex-start">
          <el-card style="margin-top: 20px; width: 90%; display: inline-block">
            <template #header><div style="text-align: left">◤</div></template>
            <div class="adviceBox">{{ advice }}</div>
            <template #footer><div style="text-align: right">◢</div></template>
          </el-card>
          <el-button class="adviceButton" @click="getAdvise"
            ><div
              style="
                margin-left: -8px;
                font-size: 25px;
                font-family: siyuanheiti;
              "
            >
              获<br />取<br />建<br />议
            </div></el-button
          >
        </div>
      </el-main>
      <el-dialog v-model="showRemarkDialog">
        <h3>请为&nbsp;{{ selectedPoi.title }}&nbsp;添加备注</h3>
        <el-card>
          <el-input
            class="remarkInput"
            v-model="remarkInput"
            type="text"
            placeholder="请输入备注内容"
          />
        </el-card>
        <el-button style="margin-top: 30px" @click="saveRemark"
          >保存备注</el-button
        >
        <el-button style="margin-top: 30px" @click="closeRemarkDialog"
          >关闭</el-button
        >
      </el-dialog>
      <el-dialog v-model="showImgDialog">
        <el-card>
          <img class="dialogImg" src="@/assets/test.jpg" />
        </el-card>
      </el-dialog>
    </el-container>
  </el-container>
</template>


<script>
import axios from "axios";
import MyHeader from "@/components/MyHeader";

const BMap = window.BMap;
const { BMAP_ANCHOR_TOP_RIGHT, BMAP_ANCHOR_BOTTOM_RIGHT } = window;
export default {
  data() {
    return {
      selectedPoi: JSON.parse(localStorage.getItem("selectedPoi")),
      map: null,
      radius: 100, // 圆的半径
      facilities: [
        { name: "工厂", distance: 4, color: "#b39148" },
        { name: "医院", distance: 3, color: "#c7ac72" },
        { name: "学校", distance: 2, color: "#dbc490" },
        { name: "公园", distance: 1, color: "#eadbb9" },
      ],
      showImgDialog: false,
      showRemarkDialog: false,
      showBottomImg: false,
      BottomImgText: "显示识别结果",
      remarkInput: "",
      advice: "右侧点击即可获取",
      //修改2
      streetview: "",
      analyseresult: "",
      streetViewInfo:{}
      //修改2结束
    };
  },
  components: {
    MyHeader,
  },
  //修改3
  computed: {
    imageSrc() {
      return `/street/${this.streetview}`;
    },
    resultSrc() {
      return `/result/${this.analyseresult}`;
    },
  },
  //修改3结束
  mounted() {
    this.initMap();
    this.addMark();
    this.openInfoWindow(this.selectedPoi);
    console.log(this.selectedPoi);
    this.getSortedFacilities();
    this.fetchDistancesAndEvaluation(); // 调用这个方法来获取数据
    //修改4
    this.getStreetView(this.selectedPoi.point.lng, this.selectedPoi.point.lat);
    //修改4结束
  },
  methods: {
    async fetchDistancesAndEvaluation() {
      try {
        const { lng, lat } = this.selectedPoi.point;
        // 获取距离信息
        const distResponse = await axios.get(
          "http://localhost:18101/api/v1/cityInfo/getDistInfo",
          {
            params: { Lon: lng, Lat: lat },
          }
        );
        if (distResponse.data.resultCode === 1) {
          this.facilities = [
            {
              name: "工厂",
              distance: distResponse.data.factoryDist,
              color: "#b39148",
            },
            {
              name: "医院",
              distance: distResponse.data.hospitalDist,
              color: "#c7ac72",
            },
            {
              name: "学校",
              distance: distResponse.data.schoolDist,
              color: "#dbc490",
            },
            {
              name: "公园",
              distance: distResponse.data.parkDist,
              color: "#eadbb9",
            },
          ];
          this.getSortedFacilities();
        } else {
          // 如果 resultCode 不为 1，设置默认值
          this.facilities = [
            { name: "工厂", distance: 1, color: "#b39148" },
            { name: "医院", distance: 2, color: "#c7ac72" },
            { name: "学校", distance: 3, color: "#dbc490" },
            { name: "公园", distance: 4, color: "#eadbb9" },
          ];
        }
      } catch (error) {
        console.error("获取数据失败:", error);
        // 设置默认值
        this.facilities = [
          { name: "工厂", distance: 1, color: "#b39148" },
          { name: "医院", distance: 2, color: "#c7ac72" },
          { name: "学校", distance: 3, color: "#dbc490" },
          { name: "公园", distance: 4, color: "#eadbb9" },
        ];
        this.advice = "暂无";
      }
    },
    async getAdvise() {
      const { lng, lat } = this.selectedPoi.point;

      // 获取评估信息
      const evalResponse = await axios.get(
        "http://47.113.109.185:18100/api/v1/cityInfo/getPointEvaluate",
        {
          params: { Lon: lng, Lat: lat },
        }
      );

      if (evalResponse.data.resultCode === 1) {
        this.advice = evalResponse.data.evaluate;
      } else {
        // 如果 resultCode 不为 1，设置默认值
        this.advice = "暂无";
      }
    },
    async addFavorite() {
      const { lng, lat } = this.selectedPoi.point;
      const note = prompt("请输入备注"); // 获取用户输入的备注
      console.log(note)
      if (!note) {
        alert("备注不能为空！");
        return;
    }

      // 进行收藏
      const distResponse = await axios.get(
        "http://localhost:18101/api/v1/cityInfo/addFavorite",
        {
          params: {
            userId: localStorage.getItem("userID"), // 获取本地存储中的用户ID
            lat: lat,
            lon: lng,
            note: note,
          }  

        }
      );

      if (distResponse.data.resultCode === 1) {
        alert("添加收藏成功！");
      } else {
        alert("添加收藏失败！");
      }
    },
    test() {
      var poi = this.selectedPoi;
      console.log(poi.address);
    },
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
    addMark() {
      this.map.clearOverlays();
      const marker = new BMap.Marker(this.selectedPoi.point);
      this.map.addOverlay(marker);
      // 添加点击事件监听器
      marker.addEventListener(
        "click",
        function () {
          this.openInfoWindow(this.selectedPoi);
        }.bind(this)
      );
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
      const p = new BMap.Point(poi.point.lng, poi.point.lat);
      this.map.openInfoWindow(infoWindow, p);
      this.selectedRow = poi;
      this.selectedPoi = {
        title: poi.title,
        address: poi.address,
        point: {
          lng: poi.point.lng,
          lat: poi.point.lat,
        },
      };
    },
    calculateDistance(distance) {
      // 根据距离计算在圆内的半径
      return (
        (distance / Math.max(...this.facilities.map((f) => f.distance))) *
        this.radius
      );
    },
    addRemark() {
      console.log(this.showRemarkDialog);
      this.showRemarkDialog = true;
    },
    closeRemarkDialog() {
      this.showRemarkDialog = false;
    },
    saveRemark() {
      //存入后端的操作
      this.$alert("保存成功", "提示", {
        confirmButtonText: "确定",
        type: "success",
      });
      this.showRemarkDialog = false;
    },
    getSortedFacilities() {
      // 先对设施进行排序
      const sortedFacilities = this.facilities.sort(
        (a, b) => b.distance - a.distance
      );

      // 创建一个新的数组来存储合并后的设施
      const mergedFacilities = [];

      // 遍历排序后的设施列表
      for (let i = 0; i < sortedFacilities.length; i++) {
        if (
          i > 0 &&
          sortedFacilities[i].distance === sortedFacilities[i - 1].distance
        ) {
          // 如果当前设施和前一个设施的距离相同，合并名称
          mergedFacilities[mergedFacilities.length - 1].name +=
            "|" + sortedFacilities[i].name;
        } else {
          // 如果距离不同，则直接将设施添加到新数组中
          mergedFacilities.push(sortedFacilities[i]);
        }
      }
      this.facilities = mergedFacilities;
    },
    showImg() {
      this.showImgDialog = true;
    },
    //修改5
    async getStreetView(lng, lat) {
      try {
        console.log(lng);
        console.log(lat);

        const response = await fetch(
          `http://localhost:18101/api/v1/cityInfo/getStreetViewInfo?Lng=${lng}&Lat=${lat}`
        );
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log(data.english);
        this.streetViewInfo=data;
        this.streetview = data.english + ".png";
        //this.streetview = data.english;
        this.analyseresult = data.english + ".png_color.png";
        //this.streetview = "test.jpg"
        console.log(this.streetview);
        console.log(this.analyseresult);
      } catch (error) {
        console.error("Error fetching street view info:", error);
      }
    },
    //修改5结束
    formatDistance(value) {
      return Number(value).toFixed(5);
    },
    showBottomImgCard() {
      if(!this.showBottomImg){
        this.showBottomImg = true;
        this.BottomImgText="隐藏识别结果";
      }
      else{
        this.showBottomImg = false;
        this.BottomImgText="显示识别结果";
      }

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
.mapContainer {
  margin-top: -20px;
  height: 60vh;
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
.streetImg {
  margin-top: 5px;
  height: 250px;
  width: 95%;
  display: inline-block;
  transition: transform 0.3s ease;
}
.dialogImg {
  margin-top: 5px;
  height: 650px;
  display: inline-block;
  transition: transform 0.3s ease;
}
.streetImg:hover {
  cursor: pointer;
  transform: scale(1.05);
}
.LikeButton {
  background-color: #56a86b;
  color: white;
  transition: background-color 0.3s;
}
.LikeButton:hover {
  background-color: #000000;
  color: white;
}
.NoteButton {
  background-color: #000000;
  color: white;
  transition: background-color 0.3s;
}
.NoteButton:hover {
  background-color: #56a86b;
  color: rgb(0, 0, 0);
}
.pointTitle {
  margin-top: 30px;
  text-align: left;
  font-size: 40px;
  font-family: zcool-qingke-jianheiti;
}
.infoBox {
  display: flex;
  flex-direction: row;
  align-items: flex-start;
}
.subtitleBox {
  text-align: left;
  background: #000000;
  color: #ffffff;
  font-size: 15px;
  line-height: 20px;
  width: 99%;
}
.detailInfo {
  text-align: left;
  display: inline-block;
  width: 55%;
  font-size: 17px;
  line-height: 34px;
}
.facilityInfo {
  display: inline-block;
  text-align: left;
  width: 42%;
  font-size: 17px;
  line-height: 34px;
}
.infoText {
  font-size: 20px;
  line-height: 30px;
}
.container {
  display: inline-block;
  width: 48%;
  justify-content: center;
  align-items: center;
}

.circle {
  fill: none;
  stroke: #000;
  stroke-width: 1px;
}

.facility {
  fill-opacity: 1;
}
.facilityDistance {
  margin-left: 0px;
}
.name {
  text-anchor: middle;
  font-size: 12px;
}
.adviceBox {
  margin-top: 20px;
}
.remarkInput {
  height: 150px;
}
.adviceButton {
  margin-top: 20px;
  width: 5%;
  margin-left: 5px;
  min-width: 60px;
  display: inline-block;
  background-color: #000000;
  color: #ffffff;
  height: 200px;
  transition: background-color 0.3s;
}
.adviceButton:hover {
  cursor: pointer;
  color: black;
  background-color: #eb6b09;
}
/* 修改6 */
.top-image {
  position: absolute;
  top: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 100%;
  height: auto; /* 保持图片的原始宽高比 */
}
.bottom-image {
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 100%;
  height: auto;
}
.imgButton{
  width: 100%;
  height: 18px;
  color:#000000;
  font-family: siyuanheiti;
  font-weight: bold;
  background-color: #56a86b;
  transition: background-color 0.3s;
}
.imgButton:hover{
  background-color:#000000;
  color: #ffffff;
}
/* 修改6结束 */
</style>