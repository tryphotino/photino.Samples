import "./App.css";
import React from "react";
import BranchSelector from "./components/BranchSelector";
import CommitCards from "./components/CommitCards";

const API_URL = `https://api.github.com/repos/tryphotino/photino.NET/commits?per_page=3&sha=`;

class App extends React.Component {
    constructor(props) {
        super(props);

        this.branches = ["master", "debug"];
        this.state = {
            currentBranch: "",
            commits: [],
        };

        this.selectBranch = this.selectBranch.bind(this);
    }

    componentWillMount() {
        this.selectBranch(this.branches[0]);
    }

    async selectBranch(branch) {
        this.setState({ currentBranch: branch });
        this.setState({ commits: await this.fetchCommits(branch) });
    }

    async fetchCommits(branch) {
        const url = `${API_URL}${branch}`;
        return await (await fetch(url)).json();
    }

    render() {
        return (
            <div id="content">
                <h1>Latest Photino.NET Commits</h1>
                <BranchSelector
                    options={this.branches}
                    value={this.state.currentBranch}
                    onChange={async (branch) => await this.selectBranch(branch)}
                />
                <p>tryphotino/photino.NET @{this.state.currentBranch}</p>
                <CommitCards commits={this.state.commits} />
            </div>
        );
    }
}

export default App;
