const HTMLInlineCSSWebpackPlugin = require("html-inline-css-webpack-plugin").default;
const HtmlInlineScriptPlugin = require('html-inline-script-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const path = require('path');

module.exports = {
    devtool: 'inline-source-map',
    entry: {
        index: './src/index.ts'
    },
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: [MiniCssExtractPlugin.loader, 'css-loader']
            },
            {
                test: /\.(ts|tsx)$/i,
                loader: 'ts-loader',
                exclude: ['/node_modules/']
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2|png|jpg|gif)$/i,
                type: 'asset/inline'
            },
            {
                test: /\.js$/i,
                loader: 'babel-loader',
                options: {
                    presets: [
                        ['@babel/preset-env', { targets: { "ie": 11 } }]
                    ]
                }
            }
        ]
    },
    output: {
        clean: true,
        filename: 'index.js',
        path: path.resolve(__dirname, 'dist')
    },
    plugins: [
        new HtmlWebpackPlugin({
            chunks: ['index'],
            filename: 'index.html',
            inject: 'body',
            template: './src/index.html'
        }),
        new HtmlInlineScriptPlugin({
            htmlMatchPattern: [/index\.html/],
            scriptMatchPattern: [/\.js$/]
        }),
        new MiniCssExtractPlugin(),
        new HTMLInlineCSSWebpackPlugin()
    ],
    resolve: {
        alias: {
            'react': 'preact/compat',
            'react-dom/test-utils': 'preact/test-utils',
            'react-dom': 'preact/compat',
            'react/jsx-runtime': 'preact/jsx-runtime'
        },
        extensions: ['.tsx', '.ts', '.jsx', '.js', '...']
    }
};