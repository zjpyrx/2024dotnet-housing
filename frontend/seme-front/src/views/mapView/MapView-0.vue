<template>
  <div>地图页面</div>
  <div class="map-container">
    <label>关键词：<input v-model="keyword" /></label>
    <label>地区：<input v-model="location" /></label>

    <baidu-map
      class="bm-view"
      :center="{ lng: 116.404, lat: 39.915 }"
      :zoom="15"
      :scroll-wheel-zoom="true"
    >
      <bm-navigation anchor="BMAP_ANCHOR_BOTTOM_RIGHT"></bm-navigation>
      <bm-scale anchor="BMAP_ANCHOR_TOP_RIGHT"></bm-scale>
      <bm-local-search
        :keyword="keyword"
        :auto-viewport="true"
        :location="location"
      ></bm-local-search>
    </baidu-map>
  </div>
</template>
   
  <script>
export default {
  data() {
    return{
      location: '北京',
      keyword: '百度'
    }
  },
  methods: {
    getpoint() {
      console.log("clicked");
    },
  },
};
</script>
  <style scoped>
.bm-view {
  width: 100%;
  height: 100%;
}
.map-container {
  height: 80vh;
  width: 80%;
}
</style>