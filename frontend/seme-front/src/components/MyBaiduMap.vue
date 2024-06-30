<template>
    <p>搜索地址获取地址的经纬度</p>
    <el-form label-width="80px">
          <el-row>
            <el-col :span="10">
              <el-form-item label="当前地点">
                <el-input
                  size="small"
                  type="text"
                  id="suggestId"
                  v-model="addressInfo.address"
                  placeholder="请输入地点"
                />
              </el-form-item>
            </el-col>
            <el-col :span="7">
              <el-form-item label="位置经度">
                <el-input
                  size="small"
                  type="text"
                  v-model="addressInfo.longitude"
                  readonly
                />
              </el-form-item>
            </el-col>
            <el-col :span="7">
              <el-form-item label="位置纬度">
                <el-input
                  size="small"
                  type="text"
                  v-model="addressInfo.latitude"
                  readonly
                />
              </el-form-item>
            </el-col>
          </el-row>
        </el-form>
        <div
          class="baidu"
          ref="mapRef"
          id="baidumap"
          style="width: 100%; height: 450px"
        ></div>
      
</template>
<script>
export default {
  data() {
    return {
      addressInfo: {
        longitude: "1", // 经度
        latitude: "1", // 纬度
        province: "1", // 省
        city: "1", // 市
        district: "1", // 区
        address: "1" // 详细地址
      }
    };
  }
}
</script>