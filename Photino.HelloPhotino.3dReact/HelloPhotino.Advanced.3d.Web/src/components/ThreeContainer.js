import React, { Component } from 'react';
import ThreeEntryPoint from '../threejs/ThreeEntryPoint';

export default class ThreeContainer extends Component {

  componentDidMount() {
    ThreeEntryPoint(this.scene);
  }

  render() {
    return (
      <>
        <div ref={element => this.scene = element} />
      </>
    );
  }
}