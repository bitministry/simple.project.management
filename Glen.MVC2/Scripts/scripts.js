function MyDate(date) {
    var dd = new Date(date);
    return dd.getFullYear() + "-" + ( dd.getMonth()+ 1 ) + "-" + dd.getDate();
}