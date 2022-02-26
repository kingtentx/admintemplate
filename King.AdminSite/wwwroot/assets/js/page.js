
function initData(type, itemId, tagsid, cur_page) {
    //console.log(type, itemId, tagsid, cur_page)
    var type = $.trim(type);
    var obj = {};
    switch (type) {
        case 'album':
            obj = {
                api: '/home/getalbums',
                pageIndex: 1,
                pageSize: 3
            };
            break;
        case 'articles':
            obj = {
                api: '/home/getarticles',
                pageIndex: 1,
                pageSize: 5
            };
            break;
        case 'jobs':
            obj = {
                api: '/home/getjobs',
                pageIndex: 1,
                pageSize: 1
            };
            break;
    }

    $.get(obj.api + '?tid=' + tagsid + '&pageIndex=' + cur_page + '&pageSize=' + obj.pageSize, function (res) {
        $('#' + type + '-list-' + itemId).find('#list-' + itemId + '-' + tagsid).html('');
        renderList(res.data, type, itemId, tagsid);

        $('#' + type + '-list-' + itemId).find('#page-' + itemId + '-' + tagsid).html('');
        var total = res.count % obj.pageSize == 0 ? res.count / obj.pageSize : Math.ceil(res.count / obj.pageSize);
        if (total > 1)
            renderPage(cur_page, total, type, itemId, tagsid);
    });

}

function renderList(data, type, itemId, tagsid) {
    var result = '';
    switch (type) {
        case 'album':
            for (var i = 0; i < data.length; i++) {
                result += '<div class="am-u-md-4 am-u-sm-6"><a href="#"><img src="' + data[i].imageUrl + '"  alt=""></a></div>';
            }
            break;
        case 'articles':
            for (var i = 0; i < data.length; i++) {
                if (data[i].imageUrl != null) {
                    result += '<li class="am-g am-list-item-desced am-list-item-thumbed am-list-item-thumb-left">'
                        + '<div class="am-u-sm-4 am-list-thumb"><a href="/home/article/info-' + data[i].articleId + '.html" target="_blank"><img src="' + data[i].imageUrl + '" alt="' + data[i].title + '" /></a></div>'
                        + '<div class=" am-u-sm-8 am-list-main">'
                        + '<h2 class="am-list-item-hd"><a href="/home/article/info-' + data[i].articleId + '.html" target="_blank">' + data[i].title + '</a></h2>'
                        + '<div class="am-list-item-text" style="margin-top:10px;">' + data[i].createTime + '</div>'
                        + '<div class="am-list-item-text">' + data[i].description + '</div></div></li>';
                } else {
                    result += '<li class="am-g am-list-item-desced"><div class=" am-list-main">'
                        + ' <h2 class="am-list-item-hd"><a href="/home/article/info-' + data[i].articleId + '.html" target="_blank">' + data[i].title + '</a></h2>'
                        + '<div class="am-list-item-text" style="margin-top:10px;">' + data[i].createTime + '</div>'
                        + '<div class="am-list-item-text">' + data[i].description + '</div>'
                        + '</div></li>';
                }
            }
            break;
        case 'jobs':
            for (var i = 0; i < data.length; i++) {
                result += '<div class="am-panel am-panel-default"><div class="am-panel-hd"><h4 class="am-panel-title">' + data[i].jobName + '</h4></div>'
                    + '<div id="do-not-say" class="am-panel-collapse">'
                    + '<div class="am-panel-bd"><div class="vacancies--item_content js-accordion--pane_content " style="">' + data[i].detail + '  </div>'
                    + '</div></div>';
            }
            break;
    }
    $('#' + type + '-list-' + itemId).find('#list-' + itemId + '-' + tagsid).append(result);
}

function renderPage(cur_page, t_page, type, itemId, tagsid) {
    var result = '';
    //prev
    if (cur_page > 1) {
        result += '<li><span style="cursor: pointer;" onclick="initData(\'' + type + '\',' + itemId + ',' + tagsid + ',' + (parseInt(cur_page) - 1) + ')">上一页</span></li>';
    }
    else {
        result += '<li><span>上一页</span></li>';
    }
    //page
    var size = 10;
    var start = cur_page % size > 0 ? parseInt(cur_page / size) * size + 1 : parseInt(cur_page / size) * size + 1 - size;
    var end = start + size;
    if (end > t_page) {
        end = start + (t_page % size);
    }
    //console.log(start, end, cur_page);
    for (var i = start; i < end; i++) {
        if (i == cur_page) {
            result += '<li class="am-active"><span>' + i + '</span></li>';
        } else {
            result += '<li><span style="cursor: pointer;" onclick="initData(\'' + type + '\',' + itemId + ',' + tagsid + ',' + i + ')">' + i + '</span></li>';
        }
    }
    //next
    if (cur_page < t_page) {
        result += '<li><span style="cursor: pointer;" onclick="initData(\'' + type + '\',' + itemId + ',' + tagsid + ',' + (parseInt(cur_page) + 1) + ')">下一页</span></li>';
    }
    else {
        result += '<li><span>下一页</span></li>';
    }

    $('#' + type + '-list-' + itemId).find('#page-' + itemId + '-' + tagsid).append(result);

}

$('.imgLi').on('click', function () {
    var arr = $(this).attr('id').split('-');
    initData(arr[0], parseInt(arr[1]), parseInt(arr[2]), 1);
});


