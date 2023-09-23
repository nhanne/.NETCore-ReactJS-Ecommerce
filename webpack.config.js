const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = {
    entry: "./wwwroot/js/store.js",

    output: {
        path: path.join(__dirname, 'wwwroot/build'), // Thư mục chứa file được build ra
        filename: "bundle.js" // Tên file được build ra
    },

    module: {
        rules: [
            {
                test: /\.js$/, // Sẽ sử dụng babel-loader cho những file .js
                exclude: /(node_modules|wwwroot\/myscript)/, // Loại trừ thư mục node_modules và wwwroot/js/myscript
                use: ["babel-loader"]
            },
            {
                test: /\.js$/, // Sử dụng javascript-loader cho các tệp .js trong thư mục wwwroot/js/myscript
                include: /wwwroot\/myscript/,
                use: ["javascript-loader"]
            },
            {
                test: /\.css$/, // Sử dụng style-loader, css-loader cho file .css
                use: ["style-loader", "css-loader"]
            }
        ]
    },

    plugins: [
        new HtmlWebpackPlugin({
            template: "./Views/Home/Store.cshtml"
        })
    ]
};
