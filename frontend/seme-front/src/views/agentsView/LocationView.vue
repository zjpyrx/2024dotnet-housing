<template>
    <div>
      <button @click="getLocation">获取</button>
      <div v-if="location">
        <p>IP: {{ location.ip }}</p>
        <p>城市: {{ location.city }}</p>
        <p>区域: {{ location.region }}</p>
        <p>国家: {{ location.country }}</p>
        <p>位置: {{ location.loc }}</p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        location: null,
      };
    },
    methods: {
      async getIPAddress() {
        try {
          const response = await axios.get('https://api.ipify.org?format=json');
          return response.data.ip;
        } catch (error) {
          console.error('获取IP地址时出错:', error);
          return null;
        }
      },
      async getLocation() {
        try {
          const ip = await this.getIPAddress();
          if (ip) {
            const response = await axios.get('http://localhost:18101/api/v1/location/current', {
              params: { ipAddress: ip },
            });
            this.location = response.data;
            console.log(this.location.loc)
          }
        } catch (error) {
          console.error('获取位置数据时出错:', error);
        }
      },
    },
  };
  </script>
  
  <style scoped>
  button {
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
  }
  
  div {
    margin-top: 20px;
  }
  
  p {
    font-size: 14px;
  }
  </style>
  