<script>

import axios from 'axios';
export default {
  data() {
    return {
      selectedCategory: '?',
      selectedSubcategory: '?',
      location: '',
      recommendations: [],
      function: 0,
      categories: [
        { name: '商业服务业设施用地', value: 'C' },
        { name: '娱乐康体用地', value: 'CE' },
        { name: '办公用地', value: 'OB' },
        { name: '医疗卫生用地', value: 'HM' },
        { name: '教育科研用地', value: 'ED' },
        { name: '社区服务设施用地', value: 'CS' },
        { name: '其他', value: '?'}
      ],
      subcategories: {
        C: [
          { name: '零售商业用地', value: 'CR' },
          { name: '餐饮用地', value: 'CF' },
          { name: '住宿用地', value: 'CL' },
          { name: '其他商业服务用地', value: 'CO' }
        ],
        CE: [
          { name: '文化娱乐用地', value: 'CEC' },
          { name: '体育用地', value: 'CET' }
        ],
        OB: [
          { name: '企业办公用地', value: 'OBE' },
          { name: '政府办公用地', value: 'OBG' },
          { name: '共享办公用地', value: 'OBS' }
        ],
        HM: [
          { name: '综合医院用地', value: 'HMG' },
          { name: '社区医疗用地', value: 'HMC' },
          { name: '康复和护理用地', value: 'HMR' }
        ],
        ED: [
          { name: '基础教育用地', value: 'EDF' },
          { name: '高等教育用地', value: 'EDH' },
          { name: '职业培训用地', value: 'EDV' },
          { name: '科研用地', value: 'EDR' }
        ],
        CS: [
          { name: '社区服务用地', value: 'CSC' },
          { name: '福利设施用地', value: 'CSF' },
          { name: '文化设施用地', value: 'CSU' }
        ],
        '?': [
          { name: '其他1', value: '1' },
          { name: '其他2', value: '2' },
          { name: '其他3', value: '3' },
          { name: '其他4', value: '4' }
        ]
      }
    };
  },
  methods: {
    getSubcategories(category) {
      return this.subcategories[category] || [];
    },
    async getRecommendations() {
      const response = await axios.post('/api/recommendations', {
        category: this.selectedCategory,
        subcategory: this.selectedSubcategory,
        location: this.location
      });
      this.recommendations = response.data;
    }
  }
}; // end of export
</script>
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
              @click="showHousingAnalysisViewDetailTest(selectedPoiTest)"
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

      <el-aside width="200px" height="900px">
        <el-menu
          default-active="1"
          class="el-menu-vertical-demo"
          background-color="#e3ddcf"
          text-color="#000000"
          active-text-color="#000000"
        >
          <el-menu-item index="1" @click="index = 1">商业地理搜索</el-menu-item>
          <el-menu-item index="2" @click="index = 2">商业用地推荐</el-menu-item>
          <el-menu-item index="3" @click="index = 3">商业用地评估</el-menu-item>

        </el-menu>
      </el-aside>

          <!--当function分别为1,2,3时，无法显示不同的内容-->
          <el-container v-if="index === 1">    
            <el-main>
              <h1>商业地理搜索</h1>
              <el-select v-model="selectedCategory" placeholder="选择类别" style="width: 200px">
                <el-option
                  v-for="category in categories"
                  :key="category.value"
                  :label="category.name"
                  :value="category.value"
                />
              </el-select>
              <input type="text" v-model="keyword" placeholder="输入关键词" class="keywordInput" />
              <button @click="search" class="searchButton">搜索</button>
              <div v-if="results.length">
                <table class="addressTable">
                  <tr>
                    <th>名称</th>
                    <th>地址</th>
                    <th>距离</th>
                  </tr>
                  <tr
                    v-for="result in results"
                    :key="result.id"
                    :class="{ 'result-row': true, 'result-row-selected': result === selectedResult }"
                    @click="selectResult(result)"
                  >
                    <td>{{ result.name }}</td>
                    <td>{{ result.address }}</td>
                    <td>{{ result.distance }}</td>
                  </tr>
                </table>
              </div>
            </el-main>
          </el-container>
        <el-container v-else-if="index === 2">
          <el-main>
            <h1>商业地理分析</h1>
            <el-select v-model="selectedCategory" placeholder="选择类别" style="width: 200px">
              <el-option
                v-for="category in categories"
                :key="category.value"
                :label="category.name"
                :value="category.value"
              />
            </el-select>
            <el-select v-model="selectedSubcategory" placeholder="选择子类别" style="width: 200px">
              <el-option
                v-for="subcategory in getSubcategories(selectedCategory)"
                :key="subcategory.value"
                :label="subcategory.name"
                :value="subcategory.value"
              />
            </el-select>
            <input type="text" v-model="location" placeholder="输入地址或选择位置" />
            <button @click="getRecommendations">获取推荐</button>
            <div v-if="recommendations.length">
              <h2>推荐结果</h2>
              <ul>
                <li v-for="recommendation in recommendations" :key="recommendation">
                  {{ recommendation }}
                </li>
              </ul>
            </div>
          </el-main>
        </el-container>
        <el-container v-else-if="index === 3">
          <el-main>
            <h1>商业用地评估</h1>
            <input type="text" v-model="location" placeholder="输入地址或选择位置" />
            <input type="text" v-model="area" placeholder="用地需求" />
            <!--所属商业类别-->
            <el-select v-model="selectedCategory" placeholder="选择类别" style="width: 200px">
              <el-option
                v-for="category in categories"
                :key="category.value"
                :label="category.name"
                :value="category.value"
              />
            </el-select>
            <button @click="getAssessment">获取评估</button>
            <div v-if="assessment">
              <h2>评估结果</h2>
              <p>{{ assessment }}</p>
            </div>
          </el-main>
        </el-container>
    </el-container>
</template>


<style scoped>
.mapContainer {
  margin-top: -20px;
  height: 90vh;
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
  margin-top: -25px;
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


.searchBar {
  display: flex;
  align-items: center;
}

.headerContainer {
  display: flex;
  flex-direction: column;
}
</style>