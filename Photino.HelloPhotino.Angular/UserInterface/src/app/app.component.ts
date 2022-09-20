import { Component } from '@angular/core';

const API_URL = `https://api.github.com/repos/tryphotino/photino.NET/commits?per_page=3&sha=`;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  branches = ['master', 'debug'];
  currentBranch: string | undefined;
  commits: any[] = [];

  ngOnInit() {
    this.selectBranch(this.branches[0]);
  }

  async selectBranch(branch: string) {
    this.currentBranch = branch;
    this.commits = await this.fetchCommits(branch);
  }

  async fetchCommits(branch: string) {
    const url = `${API_URL}${branch}`;
    return await (await fetch(url)).json();
  }

  truncate(v: string) {
    const newline = v.indexOf("\n");
    return newline > 0 ? v.slice(0, newline) : v;
  }

  formatDate(v: string) {
    return v.replace(/T|Z/g, " ");
  }
}
