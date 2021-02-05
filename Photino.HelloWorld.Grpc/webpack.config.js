const path = require("path");

module.exports = {
    entry: path.resolve(__dirname, 'src/TestGRpc.js'),
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist'),
        filename: 'main.js',
        library: 'SayHello'
  }
};