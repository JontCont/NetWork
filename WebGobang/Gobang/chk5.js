//檢查五子連線
function chk5(i, j, k) {
    //水平連線檢查
    var n = 0;//連線累計值
    for (m = i - 4; m <= i + 4; m++) {
        n = nn(m, j, k, n);//檢視下一格
        if (n == 5) return true;//五子連線
    }
    //垂直連線檢查
    n = 0;//累計值歸零
    for (m = j - 4; m <= j + 4; m++) {
        n = nn(i, m, k, n);//檢視下一格
        if (n == 5) return true;//五子連線
    }
    //左上到右下檢查
    n = 0;//累計值歸零
    for (m = -4; m <= 4; m++) {
        n = nn(i + m, j + m, k, n);//檢視下一格
        if (n == 5) return true;//五子連線
    }
    //右上到左下檢查
    n = 0;//累計值歸零
    for (m = -4; m <= 4; m++) {
        n = nn(i - m, j + m, k, n);//檢視下一格
        if (n == 5) return true;//五子連線
    }
    return false;//無連線
}
//檢視下一格是否連續出現指定棋種
function nn(i, j, k, n) {
    if (i < 0 || i > 18 || j < 0 || j > 18) return n;//超出棋盤不計算
    if (A[i][j] == k) {
        return n + 1;//累計數值+1
    } else {
        return 0;//累計值歸0
    }
}