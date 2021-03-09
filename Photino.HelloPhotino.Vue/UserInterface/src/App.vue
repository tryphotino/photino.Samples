<template>
  <div id="app">
    <img class="logo center" src="./assets/photino-logo.svg" alt="Photino Logo">
    <HelloPhotino msg="Hello to Photino.Vue"/>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import HelloPhotino from './components/HelloPhotino.vue';
import './assets/main.css';

export default Vue.extend({
  name: 'App',
  components: {
    HelloPhotino,
  },
});
</script>
