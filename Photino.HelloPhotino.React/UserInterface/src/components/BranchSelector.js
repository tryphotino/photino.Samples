import React from "react";

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

export default BranchSelector;
