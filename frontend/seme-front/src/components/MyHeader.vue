<template>
  <div class="myHeader">
    <div style="display: flex; align-items: center">
      <div style="display: inline-block">
        <img
          src="@/assets/logo/myLogo.png"
          style="
          margin-left:10px;
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
              <tr @click="gotoUserInfo()">
                <td>
                  <el-icon><Location /></el-icon>&nbsp;&nbsp;个人信息
                </td>
              </tr>
              <tr @click="jumpToFav()">
                <td>
                  <el-icon><Location /></el-icon>&nbsp;&nbsp;我的收藏
                </td>
              </tr>
              <tr @click="Exit()">
                <td>
                  <el-icon><Location /></el-icon>&nbsp;&nbsp;退出登录
                </td>
              </tr>
              <tr @click="UpdateNews()">
                <td>
                  <el-icon><Location /></el-icon>&nbsp;&nbsp;更新新闻
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <template #reference>
          <el-button class="avaterButton" circle>
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
          <td @click="jumpToMapView">-街景浏览-</td>
          <td @click="jumpToCommunity">-房源浏览-</td>
          <td @click="jumpToAgent">-查找中介-</td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { ElMessage } from "element-plus"; // 使用 Element Plus 的消息组件

export default {
  methods:{
    back(){
      this.$router.push('/HomeView');
    },
    jumpToFav(){
      this.$router.push('/FavoritesView');
    },
    jumpToMapView() {
      this.$router.push("/MapView");
    },
    async UpdateNews(){
      const res = await axios.get(
          "http://localhost:18101/api/v1/cityInfo/scrape"
        );
        if (res.data.resultCode === 1){
          ElMessage.success("更新成功");
          this.$router.push('HomeView');
        }
        else{
          ElMessage.success("更新失败");
        }
    },
    jumpToAgent(){
      this.$router.push('/AgentsView');
    },
    jumpToCommunity(){
      this.$router.push('/CommunityView');
    }
  }
};
</script>

<style scoped>
.myHeader {
  text-align: left;
  height: 75px;
  min-width: 1300px;
  width:100%;
  background-color: #000000;
  background-image: url("@/assets/header3.jpg");
  background-size: cover;
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
.popoverTable {
  width: 100%;
}
.popoverTable tr {
  height: 30px;
  color: #ffffff;
  transition: background-color 0.3s;
}
.popoverTable tr:hover {
  cursor: pointer;
  color: #000000;
  background-color: #e3ddcf;
}
.naviBar {
  background-color: #56a86b;
  color: white;
  border-collapse: collapse;
  width: 60%;
  text-align: center;
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
</style>