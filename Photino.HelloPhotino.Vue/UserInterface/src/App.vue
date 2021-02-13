<template>
  <div id="app">
    <img alt="Vue logo" src="../assets/logo.png">
    <HelloPhotinoVue msg="Welcome to your Vue.js + TypeScript App"/>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import HelloPhotinoVue from './components/HelloPhotino.vue';

export default Vue.extend({
  name: 'App',
  components: {
    HelloPhotinoVue,
  },
});
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
