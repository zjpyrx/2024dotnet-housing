<template>
  <el-header>
    <MyHeader></MyHeader>
  </el-header>
  <el-container>
    <el-aside width="500px">
      <div class="weightsContainer">
          <div class="weightsCard" style="margin-top: -30px; margin-left: -100px">
            权重调整
          </div>
          <div style="margin-top: 40px; width: 90%; margin-left: 20px">
            <div class="inputBox">
              <div class="inputFront">公园绿地</div>
              <input
              v-model.number="weights.distance0505"
              class="userInput"
              placeholder="输入权重"
            />
            </div>
            <div class="inputBox">
              <div class="inputFront">工业用地</div>
              <input
              v-model.number="weights.distance0301"
              class="userInput"
              placeholder="输入权重"
            />
            </div>
            <div class="inputBox">
              <div class="inputFront">教育用地</div>
              <input
              v-model.number="weights.distance0502"
              class="userInput"
              placeholder="输入权重"
            />
            </div>
            <div class="inputBox">
              <div class="inputFront">医疗用地</div>
              <input
              v-model.number="weights.distance0503"
              class="userInput"
              placeholder="输入权重"
            />
            </div>
            <el-button
            class="weightsButton"
            @click="applyWeights"
            >应用权重</el-button
          >
          </div>
        </div>
    </el-aside>
    <el-main>
      <div class="MyMainContainer">
        <div class="weightsCard">
            用户收藏
          </div>
        <div style="display: flex; justify-content: center">
          <table class="myTable">
      <thead style="background-color: #000000;color:#e3ddcf">
        <tr>
          <th>User ID</th>
          <th>Favorite ID</th>
          <th>Longitude</th>
          <th>Latitude</th>
          <th>Note</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr class="myTr" v-for="favorite in favorites" :key="favorite.favorite_id">
          <td>{{ favorite.userId }}</td>
          <td>{{ favorite.favoriteId }}</td>
          <td>{{ favorite.longitude }}</td>
          <td>{{ favorite.latitude }}</td>
          <td>{{ favorite.note }}</td>
          <td>
            <button class="deleteButton" @click="deleteFavorite(favorite.favoriteId)">删除</button>
          </td>
        </tr>
      </tbody>
    </table>
        </div>
        <div
          style="
            flex: 1;
            padding-left: 10px;
            display: flex;
            justify-content: center;
          "
        >
          <div
            ref="chart"
            style="width: 100%; max-width: 500px; height: 300px"
          ></div>
        </div>
      </div>
    </el-main>
  </el-container>
</template>

<script>
import axios from "axios";
import { ElMessageBox, ElMessage } from "element-plus";
import * as echarts from "echarts";
import MyHeader from "@/components/MyHeader";

export default {
  name: "FavoritesView",
  data() {
    return {
      favorites: [],
      weights: {
        distance0505: 1,
        distance0301: 1,
        distance0502: 1,
        distance0503: 1,
      },
    };
  },
  components: {
    MyHeader,
  },
  mounted() {
    this.fetchFavorites();
    this.initChart();
    this.applyWeights();
  },
  methods: {
    async fetchFavorites() {
      try {
        const response = await axios.get(
          "http://localhost:18101/api/v1/cityInfo/getUserFavorite",
          {
            params: { userId: localStorage.getItem("userID") },
          }
        );
        if (response.data.resultCode === 1) {
          this.favorites = response.data.value;
        } else {
          ElMessage.error("获取收藏列表失败");
        }
      } catch (error) {
        console.error(error);
        ElMessage.error("获取收藏列表失败");
      }
    },
    async deleteFavorite(favorite_id) {
      ElMessageBox.confirm("此操作将永久删除该收藏, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(async () => {
          try {
            const response = await axios.delete(
              "http://localhost:18101/api/v1/cityInfo/deleteUserFavorite",
              {
                params: { favoriteId: favorite_id },
              }
            );
            if (response.data.resultCode === 1) {
              this.favorites = this.favorites.filter(
                (f) => f.favoriteId !== favorite_id
              );
              ElMessage.success("删除成功");
            } else {
              ElMessage.error("删除失败");
            }
          } catch (error) {
            console.error(error);
            ElMessage.error("删除失败");
          }
        })
        .catch(() => {
          ElMessage.info("已取消删除");
        });
    },
    applyWeights() {
      const sortedFavorites = [...this.favorites].sort((a, b) => {
        const weightedA =
          a.distance0505 * this.weights.distance0505 +
          a.distance0301 * this.weights.distance0301 +
          a.distance0502 * this.weights.distance0502 +
          a.distance0503 * this.weights.distance0503;
        const weightedB =
          b.distance0505 * this.weights.distance0505 +
          b.distance0301 * this.weights.distance0301 +
          b.distance0502 * this.weights.distance0502 +
          b.distance0503 * this.weights.distance0503;
        return weightedB - weightedA;
      });
      this.favorites = sortedFavorites;
      this.updateChart();
    },
    initChart() {
      this.chart = echarts.init(this.$refs.chart);
      this.updateChart();
    },
    updateChart() {
      const option = {
        series: [
          {
            name: "Weights",
            type: "pie",
            radius: ["40%", "70%"],
            avoidLabelOverlap: false,
            label: {
              show: false,
              position: "center",
            },
            emphasis: {
              label: {
                show: true,
                fontSize: "30",
                fontWeight: "bold",
              },
            },
            labelLine: {
              show: false,
            },
            data: [
              { value: this.weights.distance0505, name: "公园绿地" },
              { value: this.weights.distance0301, name: "工业用地" },
              { value: this.weights.distance0502, name: "教育用地" },
              { value: this.weights.distance0503, name: "医疗用地" },
            ],
          },
        ],
      };
      this.chart.setOption(option);
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
  min-width: 800px;
  background-color: #f4f1eb;
  height: 100vh;
  background-image: url("@/assets/pointSearch.jpg");
  background-size: cover;
}
.MyMainContainer{
  text-align: center;
  background-color: #e3ddcfa2;
}
.weightsContainer {
  border: 3px solid #000000;
  display: flex;
  width: 400px;
  height: 520px;
  flex-direction: column;
  margin-left: 50px;
  margin-top: 10%;
}
.weightsCard {
  background: radial-gradient(circle, #e3ddcf 30%, rgba(0, 0, 0, 0) 70%);
  color: #000000;
  font-size: 38px;
  font-family: zcool-qingke-jianheiti;
}
.inputBox {
  width: 300px;
  display: flex;
  align-items: center;
  margin-top: 40px;
}
.inputFront {
  display: inline-block;
  width: 100px;
  text-align: center;
  padding: 5px 0;
  display: inline-block;
  font-size: 20px;
  font-family: siyuanheiti;
  font-weight: bold;
  color:#000000
}
.userInput {
  flex: 1;
  margin-left: 10px;
  height: 25px;
  border: 2px solid #000000;
  background-color: #cfc8b8e8;
  font-size: 15px;
}
.weightsButton {
  margin-top:20px;
  margin-left: 140px;
  background-color: #000000;
  color: #e3ddcf;
  width: 100px;
  height: 40px;
  transition: background-color 0.3s;
  font-size: 20px;
  font-family: siyuanheiti;
}
.weightsButton:hover {
  background-color: #56a86b;
  color: #ffffff;
}
.deleteButton{
  background-color: #000000;
  color: #e3ddcf;
  transition: background-color 0.3s;
  font-family: siyuanheiti;
}
.deleteButton:hover {
  background-color: #ff5f5f;
  color: #ffffff;
}
h1 {
  text-align: center;
  margin-bottom: 20px;
}

.myTable {
  width: 90%;
  margin-top: 20px;
  background-color: #ffffff62;
}
.myTr{
  height: 40px;
  transition: background-color 0.3s;
}
.myTr:hover{
  background-color: #c3bcab;
}
.el-input {
  width: 200px;
}
</style>
