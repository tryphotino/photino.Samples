<!--
This example fetches latest Vue.js commits data from GitHub’s API and displays them as a list.
You can switch between the two branches.
-->

<!-- Adapted from https://vuejs.org/examples/#fetching-data -->

<script>
const API_URL = `https://api.github.com/repos/tryphotino/photino.NET/commits?per_page=3&sha=`;

export default {
  data: () => ({
    branches: ["master", "debug"],
    currentBranch: "master",
    commits: null,
  }),

  created() {
    // fetch on init
    this.fetchData();
  },

  watch: {
    // re-fetch whenever currentBranch changes
    currentBranch: "fetchData",
  },

  methods: {
    async fetchData() {
      const url = `${API_URL}${this.currentBranch}`;
      this.commits = await (await fetch(url)).json();
    },
    truncate(v) {
      const newline = v.indexOf("\n");
      return newline > 0 ? v.slice(0, newline) : v;
    },
    formatDate(v) {
      return v.replace(/T|Z/g, " ");
    },
  },
};
</script>

<template>
  <div id="content">
    <h1>Latest Photino.NET Commits</h1>
    <div v-for="(branch, index) in branches" :key="index">
      <input
        type="radio"
        :id="branch"
        :value="branch"
        name="branch"
        v-model="currentBranch"
      />
      <label :for="branch">{{ branch }}</label>
    </div>
    <p>tryphotino/photino.NET @{{ currentBranch }}</p>
    <ul id="commits" v-if="commits && commits.length > 0">
      <li v-for="(commit, index) in commits" :key="index">
        <div class="commit-head">
          <span class="author">
            <a :href="commit.author.html_url" target="_blank">{{
              commit.commit.author.name
            }}</a>
            &nbsp;–&nbsp;
            <a :href="commit.html_url" target="_blank">{{
              commit.sha.slice(0, 7)
            }}</a>
          </span>
          <span class="date">{{ formatDate(commit.commit.author.date) }}</span>
        </div>
        <p class="message">{{ truncate(commit.commit.message) }}</p>
      </li>
    </ul>
  </div>
</template>

<style>
#content {
  display: block;
  width: 100%;
}

a {
  text-decoration: none;
  color: #42b883;
}

input[type="radio"] {
  cursor: pointer;
}
input[type="radio"] + label {
  margin-left: 0.5em;
  cursor: pointer;
}

ul#commits {
  margin-top: 1rem;
  padding: 0;
  list-style: none inside;
}
ul#commits li {
  max-width: 500px;
  background: rgb(57, 55, 80);
  border-radius: 5px;
  line-height: 1.5em;
  margin-bottom: 20px;
  padding: 0.5rem 1rem 0.65rem;
  color: #fafafa;
}
ul#commits li .commit-head {
  display: flex;
  flex: 0 auto;
  flex-direction: row;
  place-content: center space-between;
}
ul#commits li .commit-head .author {
  font-weight: bold;
}
ul#commits li .commit-head .date {
  color: #dadada;
}
</style>
