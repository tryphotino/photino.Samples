<template>
  <div class="home">
    <img alt="Photino logo" src="../assets/photino-logo.svg">
    <HelloPhotino msg="Welcome to Your Photino.Vue App"/>
  </div>
</template>

<script>
// @ is an alias to /src
import HelloPhotino from '@/components/HelloPhotino.vue';

export default {
  name: 'Home',
  components: {
    HelloPhotino,
  },
};
</script>
