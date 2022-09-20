import React from "react";

class CommitCards extends React.Component {
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

    formatDate(v) {
        return v.replace(/T|Z/g, " ");
    }

    truncate(v) {
        const newline = v.indexOf("\n");
        return newline > 0 ? v.slice(0, newline) : v;
    }

    render() {
        return <ul id="commits">{this.createCommitCards()}</ul>;
    }
}

export default CommitCards;
