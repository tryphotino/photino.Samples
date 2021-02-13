<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <HelloPhotinoVue msg="Welcome to Your Photino Vue.js App"/>
  </div>
</template>

<script>
// @ is an alias to /src
import HelloPhotinoVue from '@/components/HelloPhotino.vue'

export default {
  name: 'Home',
  components: {
    HelloPhotinoVue
  }
}
</script>
