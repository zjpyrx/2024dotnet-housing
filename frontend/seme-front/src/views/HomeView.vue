<template>
  <el-container>
    <el-header>
      <MyHeader></MyHeader>
    </el-header>
    <el-container>
      <el-main >
        <div style="display:flex;justify-content: space-between;align-items: flex-end;">
        <!--- 功能页面大框 --->
        <div style="margin-left:100px;display:flex;width:900px;height:600px;justify-content: space-between;">
          <div style="display:flex;width:70%;">
            <el-card class="pointSearchCard" @click="jumpToMapView">
              <div>◤街 景 浏 览◢</div>
            </el-card>
          </div>
            <div style="display:flex;width:29%;flex-direction: column;justify-content: space-between;">
              <div style="display:flex;width:100%;height:31%;">
                <el-card @click="jumpToCommunity" class="HousingAnalysisCard">
                  <div>- 房源浏览 -</div>
                </el-card>
              </div>
              <div style="display:flex;width:100%;height:31%;">
                <el-card @click="jumpToAgent" class="BusinessAnalysisCard">
                  <div>- 查找中介 -</div>
                </el-card>
              </div>
              <div @click="jumpToFav" style="display:flex;width:100%;height:31%;">
                <el-card class="LikeCard">
                  <div>- 我的收藏 -</div>
                </el-card>
              </div>
            </div>
        </div>
        <!--- what's new --->
        <div style="border:2px solid #000000;display:flex;width:400px;height:570px;flex-direction:column;margin-right:100px;">         
          <div class="whatsnewCard" style="margin-top:-35px;margin-left:-70px;">
            WHAT'S NEW    
            <el-button
          @click="UpdateNews()"
          style="background-color: #000000;color: #ffffff;"
          circle
          ><el-icon><RefreshLeft /></el-icon
        ></el-button>
          </div>
          <div style="margin-top:0px;width:90%;margin-left: 20px;">  
            <!-- 使用newsData来显示新闻内容 -->  
            <div v-for="(item, index) in newsData" :key="index" @click="openLink(item.newsLink)" class="NewsRow">
              <div style="text-align: left;width:95%;margin-left: 10px;font-weight: bold;margin-top:10px;">◤<br>{{ item.newsName }}</div>
              <div style="text-align: right;width:95%;">{{ formatTimestamp(item.newsTime) }}<br>◢</div>
            </div> 
          </div>  
        </div>
      </div>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import MyHeader from "@/components/MyHeader";
import axios from "axios";
import { ElMessage } from "element-plus"; // 使用 Element Plus 的消息组件

export default {
  name: "HomeView",
  data() {  
    return {  
      newsData: [], // 用于存储从接口获取的新闻数据  
    };  
  },  
  async mounted() {
    localStorage.setItem("keyword", "东方明珠");
    
    try {  
      const response = await fetch('http://localhost:18101/api/v1/cityInfo/cityNews');  
      if (!response.ok) {  
        throw new Error(`HTTP error! status: ${response.status}`);  
      }  
      const data = await response.json(); // 解析JSON响应  
      this.newsData = data; // 将数据存入newsData变量  
      console.log(this.newsData); // 打印数据到控制台以验证  
    } catch (error) {  
      console.error('There has been a problem with your fetch operation:', error);  
    } 
  },
  components: {
    MyHeader
  },
  methods: {
    openLink(link) {
      window.open(link, '_blank');
    },
    jumpToMapView() {
      this.$router.push("/MapView");
    },
    jumpToMapViewz() {
      this.$router.push("/MapViewz");
    },
    formatTimestamp(timestamp) {  
      // 使用replace方法将'T'替换为一个空格  
      // 也可以替换为空字符串 ''  
      return timestamp.replace('T', ' ');  
      // 如果想要进一步格式化，可以使用更复杂的字符串操作或日期函数  
    },
    jumpToBA(){
      this.$router.push('/BusinessAnalysisView');
    },
    jumpToFav(){
      this.$router.push('/FavoritesView');
    },
    jumpToAgent(){
      this.$router.push('/AgentsView');
    },
    jumpToCommunity(){
      this.$router.push('/CommunityView');
    },
    async UpdateNews(){
      const res = await axios.get(
          "http://localhost:18101/api/v1/cityInfo/scrape"
        );
        if (res.data.resultCode === 1){
          ElMessage.success("更新成功");
          this.$router.push('HomeView');
          location.reload();
        }
        else{
          ElMessage.success("更新失败");
        }
    },
  },
};
</script>

<style scoped>
.el-header{
  height: 75px;
  min-width: 1300px;
  width:100%;
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
  height:1000px;
  background-color: #f4f1eb;
  background-image: url("@/assets/background.png");
  background-size: 100% auto;
}
.pointSearchCard{
  background-image: url("@/assets/pointSearch.jpg");
  color:#000000;
  display: flex;
  justify-content: center; 
  align-items: center;
  font-family: zcool-qingke-jianheiti;
  font-size: 60px;
  background-size: cover;
  margin-top: 5px;
  height: 100%;
  width: 100%;
  transition: transform 0.3s ease,background-image 0.3s;
}
.pointSearchCard:hover{
  background-image: url("@/assets/pointSearch_activated.jpg");
  color:#e3ddcf;
  cursor: pointer;
  transform: scale(1.05);

}
.BusinessAnalysisCard{
  background-image: url("@/assets/BusinessAnalysis.jpg");
  color:#e3ddcf;
  display: flex;
  justify-content: center; 
  align-items: center;
  font-family: zcool-qingke-jianheiti;
  font-size: 30px;
  background-size: cover;
  margin-top: 5px;
  height: 100%;
  width: 100%;
  transition: transform 0.3s ease,background-image 0.3s;
}
.BusinessAnalysisCard:hover{
  background-image: url("@/assets/BusinessAnalysis_activated.jpg");
  color:#000000;
  cursor: pointer;
  transform: scale(1.05);

}
.HousingAnalysisCard{
  background-image: url("@/assets/HousingAnalysis.jpg");
  color:#e3ddcf;
  display: flex;
  justify-content: center; 
  align-items: center;
  font-family: zcool-qingke-jianheiti;
  font-size: 30px;
  background-size: cover;
  margin-top: 5px;
  height: 100%;
  width: 100%;
  transition: transform 0.3s ease,background-image 0.3s;
}
.HousingAnalysisCard:hover{
  background-image: url("@/assets/HousingAnalysis_activated.jpg");
  color:#000000;
  cursor: pointer;
  transform: scale(1.05);
}
.LikeCard{
  background-color: #56a86b;
  color:#000000;
  display: flex;
  justify-content: center; 
  align-items: center;
  font-family: zcool-qingke-jianheiti;
  font-size: 30px;
  background-size: cover;
  margin-top: 5px;
  height: 100%;
  width: 100%;
  transition: transform 0.3s ease,background-color 0.3s;
}
.LikeCard:hover{
  background-color: #000000;
  color:#e3ddcf;
  cursor: pointer;
  transform: scale(1.05);
}
.whatsnewCard{
  background:radial-gradient(circle, #eceae3 60%, rgba(0, 0, 0, 0) 70%);
  color:#000000;
  font-size:50px;
  font-family: zcool-qingke-jianheiti;
}
.NewsRow{
  cursor: pointer;
  font-size: 15px;
  color:#000000;
  background-color: #b1a8933a;
  transition: background-color 0.3s;
  font-family: siyuanheiti;
  margin-top:15px;
}
.NewsRow:hover{
  color:#ffffff;
  background-color: #000000;
}
</style>


