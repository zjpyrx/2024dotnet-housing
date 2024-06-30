<template>
  <el-container style="font-family: siyuanheiti; color: black">
    <el-header class="myHeader">
      <div style="display: flex; align-items: center">
        <div style="display: inline-block">
          <img
            src="@/assets/logo/myLogo.png"
            style="
              margin-top: 5px;
              height: 40px;
              width: 40px;
              display: inline-block;
            "
          />
        </div>
        <div class="PageTitle">沪上慧视</div>
        <el-popover
          placement="bottom"
          width="200"
          trigger="hover"
          v-model:visible="popoverVisible"
          popper-style="background:#000000"
        >
          <div class="myPopover">
            <div>
              <img
                src="@/assets/avater.png"
                style="
                  margin-top: 5px;
                  height: 60px;
                  width: 60px;
                  display: inline-block;
                "
              />
            </div>
            <table class="popoverTable">
              <tbody>
                <tr
                  @click="gotoUserInfo()"
                >
                  <td>
                    <el-icon><Location /></el-icon>&nbsp;&nbsp;个人信息
                  </td>
                </tr>
                <tr
                  @click="showLikes()"
                >
                  <td>
                    <el-icon><Location /></el-icon>&nbsp;&nbsp;我的收藏
                  </td>
                </tr>
                <tr
                  @click="Exit()"
                >
                  <td>
                    <el-icon><Location /></el-icon>&nbsp;&nbsp;退出登录
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <template #reference>
            <el-button
              class="avaterButton"
              @click="showStreetViewDetailTest(selectedPoiTest)"
              circle
            >
              <el-icon><Avatar /></el-icon>
            </el-button>
          </template>
        </el-popover>
      </div>
      <div style="height: 50px; display: flex; align-items: center">
        <table class="naviBar">
          <tr>
            <td @click="back()">
              <el-icon><DArrowLeft /></el-icon>
            </td>
            <td>-页面1-</td>
            <td>-页面2-</td>
            <td>-页面3-</td>
          </tr>
        </table>
      </div>
    </el-header>
    <el-container>
      <el-aside width="500px">
        <div class="aside">
          <!---------------------------------没解决的问题：点击marker打不开InfoWindow------------------------------------------->
          <el-card
            style="
              background: #000000;
              box-shadow: 0 2px 4px rgba(0, 0, 0, 0.4);
            "
          >
            <img class="streetImg" @click="showImg" src="@/assets/test.jpg" />
          </el-card>
          <div
            style="
              margin-top: 20px;
              display: flex;
              flex-direction: row;
              color: black;
            "
          >
            <div style="width: 45%">
              <el-button class="LikeButton" circle
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
              <div style="margin-top: 15px">
                ■地址: {{ selectedPoi.address }}
              </div>
              <div>■经度: {{ selectedPoi.point.lng }}</div>
              <div>■纬度: {{ selectedPoi.point.lat }}</div>
              <div>■街道名称：</div>
              <div>■绿视率：</div>
              <div>■..................</div>
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
                  {{ facility.name }}: {{ facility.distance }}km
                </div>
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
          <el-button class="adviceButton"
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
      remarkInput: "",
      advice:
        "此处存放询问gpt得出的建议。\n你说得对，但是你说的也不完全对。从某种角度来说，你说的有一点对，可是从另一个角度看，你说得不对。也不能说是完全不对，只能说离完全对之间还有一点不对。如果忽略这点不对，哪你说的当然是对的，可是以一个更严谨的态度去审视你说的对不对，那么你说的又不是对的了。其实回头一想，你说的对之处在静止的世界中有可能是对的，你说的不对之处以形而上的观点来看有可能是错的。但是以发展的眼光看，你说得对之处在运动的时空里有可能是不对的，你说的不对之处以辩证的观点看有可能是对的。在此时此刻你说的有可能对也有可能不对，在往后未来你说的有可能不对也有可能对。总而言之，言而总之，一言以蔽之，观其全貌后论之，知其今后历来往事所以评议之，你说得对，但是你说的不对。",
    };
  },
  mounted() {
    this.initMap();
    this.addMark();
    this.openInfoWindow(this.selectedPoi);
    this.getSortedFacilities();
  },
  methods: {
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
    back() {
      this.$router.push("/MapView");
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
  },
};
</script>



<style scoped>
.mapContainer {
  margin-top: -20px;
  height: 60vh;
  width: 100%;
}
.myHeader {
  text-align: left;
  height: 74px;
  min-width: 1300px;
}
.el-header {
  background-color: #000000;
  background-image: url("@/assets/header3.jpg");
  background-size: cover;
}
.avaterButton {
  margin-top: 15px;
  margin-left: auto;
  margin-right: 100px;
  color: #000000;
  scale: 130%;
  background-color: #e3ddcf;
  transition: background-color 0.3s;
  transition: transform 0.3s;
}
.avaterButton:hover {
  transform: scale(1.15);
  background-color: #56a86b;
  color: rgb(0, 0, 0);
  border: #000000;
}
.myPopover {
  width: 100%;
  background: #000000;
}
.popoverTable{
  width: 100%;
}
.popoverTable tr{
  height:30px;
  color:#ffffff;
  transition: background-color 0.3s;
}
.popoverTable tr:hover{
  cursor: pointer;
  color:#000000;
  background-color: #e3ddcf;
}
.naviBar {
  background-color: #56a86b;
  color: white;
  border-collapse: collapse;
  width: 60%;
  text-align: center;
  margin-left: -20px;
  margin-top: -20px;
}
.naviBar td {
  font-size: 14px;
}
.naviBar td:hover {
  transition: background-color 0.3s;
}
.naviBar td:hover {
  background-color: #000000;
}
.naviBar tr:first-child td {
  border-top: 1px solid rgb(0, 0, 0);
  border-right: 1px solid rgb(0, 0, 0);
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
.PageTitle {
  color: #e3ddcf;
  width: 600px;
  display: inline-block;
  margin-left: 30px;
  font-size: 40px;
  font-family: zcool-qingke-jianheiti;
  margin-top: 5px;
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
</style>