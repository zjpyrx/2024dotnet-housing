<template>
  <div>
    <el-container>
      <el-aside class="myAside" style="width: 600px">
        <div class="loginContainer">
          <div class="loginCard" style="margin-top: -50px; margin-left: -70px">
            LOG IN
          </div>
          <div style="margin-top: 40px; width: 90%; margin-left: 20px">
            <div class="inputBox">
              <div class="inputFront">用户ID</div>
              <input class="userInput" v-model="userID" placeholder="UserID" />
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
              <el-button @click="userLogin" class="loginButton">登录</el-button>
              <el-button @click="register" class="registerButton"
                >注册</el-button
              >
            </div>
          </div>
        </div>
      </el-aside>
      <el-main class="myMain">
        <div class="logoContainer">
          <div class="myLogo"></div>
          <div class="myTitle">沪 上 慧 视</div>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script scoped>
import axios from "axios";
import { ElMessage } from "element-plus"; // 使用 Element Plus 的消息组件

export default {
  data() {
    return {
      userID: "",
      password: "",
    };
  },
  methods: {
    async userLogin() {
      try {
        const response = await axios.get(
          "http://localhost:18101/api/v1/cityInfo/login",
          {
            params: {
              userId: this.userID,
              password: this.password,
            },
          }
        );
        console.log(response.data.resultCode);
        if (response.data.resultCode === 1) {
          ElMessage.success("登录成功");
          this.$router.push("/HomeView");
          localStorage.setItem("userID", this.userID);
        } else {
          localStorage.setItem("userID", "");
          ElMessage.error("登录失败，用户名或密码错误");
        }
      } catch (error) {
        ElMessage.error("登录失败，请稍后再试");
      }
    },
    register() {
      this.$router.push("/RegisterView");
    },
  },
};
</script>

<style scoped>
.myAside {
  background-image: url("@/assets/backgroundBlack.png");
  background-size: cover;
}
.loginContainer {
  border: 3px solid #e3ddcf;
  display: flex;
  width: 500px;
  height: 450px;
  flex-direction: column;
  margin-left: 50px;
  margin-top: 25%;
}
.loginCard {
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
  color:#e3ddcf
}
.userInput {
  flex: 1;
  margin-left: 10px;
  height: 30px;
  border: 2px solid #e3ddcf;
  background-color: #cfc8b8e8;
  font-size: 15px;
}
.loginButton {
  margin-left: 30px;
  background-color: #e3ddcf;
  color: #000000;
  width: 100px;
  height: 60px;
  transition: background-color 0.3s;
  font-size: 25px;
  font-family: siyuanheiti;
  letter-spacing: 2px;
}
.loginButton:hover {
  background-color: #56a86b;
  color: #000000;
}
.registerButton {
  margin-left: 30px;
  background-color: #56a86b;
  color: #000000;
  width: 100px;
  height: 60px;
  transition: background-color 0.3s;
  font-size: 25px;
  font-family: siyuanheiti;
  letter-spacing: 2px;
}
.registerButton:hover {
  background-color: #000000;
  color: #ffffff;
}
.myMain {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-image: url("@/assets/pointSearch.jpg");
  background-size: cover;
}

.logoContainer {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 900px;
  height: 600px;
  background-color: #000000ac;
}

.myLogo {
  width: 300px;
  height: 288px;
  background-image: url("@/assets/logo/myLogo.png");
  background-size: cover;
}

.myTitle {
  color: #e3ddcf;
  width: 600px;
  text-align: center;
  font-size: 70px;
  font-family: zcool-qingke-jianheiti;
  margin-top: 20px; /* 调整标题与Logo之间的间距 */
}
</style>
