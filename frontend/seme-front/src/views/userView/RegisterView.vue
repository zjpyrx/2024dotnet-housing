<template>
  <el-container>
    <el-main class="myMain">
      <div class="outerContainer">
        <div class="registerContainer">
          <div
            class="registerCard"
            style="margin-top: -50px; margin-left: -70px"
          >
            Register
          </div>
          <div style="margin-top: 40px; width: 90%; margin-left: 20px">
            <div class="inputBox">
              <div class="inputFront">用户ID</div>
              <input class="userInput" v-model="userID" placeholder="UserID" />
            </div>
            <div class="inputBox">
              <div class="inputFront">昵称</div>
              <input class="userInput" v-model="name" placeholder="name" />
            </div>
            <div class="inputBox">
              <div class="inputFront">密码</div>
              <input
                class="userInput"
                v-model="password"
                placeholder="Password"
                type="password"
              />
            </div>
            <div style="margin-top: 20px">
              <el-button @click="back" class="registerButton">
                返回
              </el-button>
              <el-button @click="userRegister" class="registerButton"
                >注册</el-button
              >
            </div>
          </div>
        </div>
      </div>
    </el-main>
  </el-container>
</template>
  
  <script>
import axios from "axios";
import { ElMessage } from "element-plus"; // 使用 Element Plus 的消息组件

export default {
  data() {
    return {
      userID: "",
      name: "",
      password: "",
    };
  },
  methods: {
    back(){
      this.$router.push('/');  
    },
    async userRegister() {
      try {
        const response = await axios.get(
          "http://localhost:18101/api/v1/cityInfo/register",
          {
            params: {
              userId: this.userID,
              name: this.name,
              password: this.password,
            },
          }
        );

        if (response.data.resultCode === 1) {
          ElMessage.success("注册成功");
          // 在这里添加注册成功后的操作，比如跳转到登录页
        } else if (response.data.resultCode === -102) {
          ElMessage.error("用户ID已经注册过");
        } else if (response.data.resultCode === -404) {
          ElMessage.error("特殊故障，请稍后再试");
        }
      } catch (error) {
        ElMessage.error("注册失败，请稍后再试");
      }
    },
  },
};
</script>
  
  <style>
.myMain {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-image: url("@/assets/pointSearch.jpg");
  background-size: cover;
}
.outerContainer {
  margin-top:-300px;
  justify-content: center;
  align-items: center;
}
.registerContainer {
  border: 3px solid #000000;
  display: flex;
  width: 500px;
  height: 450px;
  flex-direction: column;
  margin-left: 50px;
  margin-top: 25%;
}
.registerCard {
  background: radial-gradient(circle, #000000 40%, rgba(0, 0, 0, 0) 70%);
  color: #e3ddcf;
  font-size: 70px;
  font-family: zcool-qingke-jianheiti;
}
.inputBox {
  width: 400px;
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
  font-size: 25px;
  font-family: siyuanheiti;
  color: #000000;
}
.userInput {
  flex: 1;
  margin-left: 10px;
  height: 30px;
  border: 2px solid #000000;
  background-color: #e3ddcf;
  font-size: 15px;
}
.registerButton {
  margin-left: 30px;
  background-color: #000000;
  color: #ffffff;
  width: 100px;
  height: 60px;
  transition: background-color 0.3s;
  font-size: 25px;
  font-family: siyuanheiti;
  letter-spacing: 2px;
}
.registerButton:hover {
  background-color: #56a86b;
  color: #000000;

}
</style>
  