import React from "react";
import "./App.css";

const API_URL = `https://api.github.com/repos/tryphotino/photino.NET/commits?per_page=3&sha=`;

class BranchSelector extends React.Component {
    constructor(props) {
        super(props);

        if (props.options.length === 0) {
            throw new Error("No options in BranchSelector.");
        }

        this.handleRadioChange = this.handleRadioChange.bind(this);
    }

    handleRadioChange(event) {
        this.props.onChange(event.target.value);
    }

    createOptions() {
        return this.props.options.map((option, index) => (
            <div key={"option-" + index}>
                <input
                    type="radio"
                    id={"option-" + option}
                    name="branch"
                    value={option}
                    checked={this.props.value === option}
                    onChange={this.handleRadioChange}
                />
                <label htmlFor={"option-" + option}>{option}</label>
            </div>
        ));
    }

    render() {
        return this.createOptions();
    }
}

class CommitView extends React.Component {
    formatDate(v) {
        return v.replace(/T|Z/g, " ");
    }

    truncate(v) {
        const newline = v.indexOf("\n");
        return newline > 0 ? v.slice(0, newline) : v;
    }

    createCommitCards() {
        return this.props.commits.map((commit, index) => (
            <li key={"commit-" + index}>
                <div className="commit-head">
                    <span className="author">
                        <a href={commit.author.html_url} target="_blank">
                            {commit.commit.author.name}
                        </a>
                        &nbsp;–&nbsp;
                        <a href={commit.html_url} target="_blank">
                            {commit.sha.slice(0, 7)}
                        </a>
                    </span>
                    <span className="date">
                        {this.formatDate(commit.commit.author.date)}
                    </span>
                </div>
                <p className="message">
                    {this.truncate(commit.commit.message)}
                </p>
            </li>
        ));
    }

    render() {
        return <ul id="commits">{this.createCommitCards()}</ul>;
    }
}

class GitHubCommitViewer extends React.Component {
    constructor(props) {
        super(props);

        this.branches = ["master", "debug"];
        this.state = {
            branch: this.branches[0],
            commits: [],
        };

        this.handleBranchChange = this.handleBranchChange.bind(this);
    }

    async handleBranchChange(branch) {
        this.setState({ branch: branch });
        await this.fetchData(branch);
    }

    async fetchData(branch) {
        const url = `${API_URL}${branch}`;
        const commits = await (await fetch(url)).json();

        this.setState({ commits: commits });

        console.log(branch, this.state.commits);
    }

    componentWillMount() {
        console.log("will mount ...");
        this.fetchData(this.state.branch);
    }

    componentDidMount() {
        console.log("did mount ...");
    }

    render() {
        return (
            <div id="content">
                <h1>Latest Photino.NET Commits</h1>
                <BranchSelector
                    options={this.branches}
                    value={this.state.branch}
                    onChange={async (branch) =>
                        await this.handleBranchChange(branch)
                    }
                />
                <p>tryphotino/photino.NET @{this.state.branch}</p>
                <CommitView commits={this.state.commits} />
            </div>
        );
    }
}

function App() {
    return <GitHubCommitViewer />;
}

export default App;
