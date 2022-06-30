const path = require("path");

module.exports = {
  entry: './App/index.tsx',
  output: {
    path: path.resolve(__dirname, "wwwroot"),
    filename: "js/bundle.js",
  },
  resolve: {
    extensions: [".js", ".jsx", ".json", ".ts", ".tsx"],
  },
  module: {
    rules: [
      { test: /\.(ts|tsx)$/, loader: "ts-loader" },
      { enforce: "pre", test: /\.js$/, loader: "source-map-loader" },
      { test: /\.css$/, use: ['style-loader', 'css-loader'] },
    ]
  },
  devServer: {
    static: {
      directory: path.join(__dirname, 'wwwroot'),
    },
    historyApiFallback: {
      rewrites: [
        { from: /\/wwwroot/, to: `/wwwroot` }
      ]
    },
    compress: true,
    port: 5000
  }
}