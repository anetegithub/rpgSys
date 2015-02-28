$(function () {

});

$('#send').click(function () {

});

function load_chat() {
    $.getJSON('../api/chat')
            .done(function (data) {
                var html = "";
                var left_right="left";
                for (var i = 0; i < data.length; i++) {
                    html += "<li class='" + left_right + " clearfix'>"
                    html += "<span class='chat-img pull-" + left_right + "'>";
                    html += "<img src='" + data.UserAvatar + "' alt='" + getCookie("pl_name") + "' class='img-circle'/></span>"
                    html += "<div class='chat-body'><strong>" + getCookie("pl_name") + "</strong><small class='pull" + left_right + " text-muted'><i class='fa fa-clock-o fa-fw'><i>" + data.Stamp.toString() + "</small><p>";
                    html += data.Text;
                    html += "</p></div></li>";
                }
                $('#chatBox').html(html);
            });
}

//                                      <li class="left clearfix">
//                                        <span class="chat-img pull-left">
//                                            <img src="img/1.png" alt="User" class="img-circle" />
//                                        </span>
//                                        <div class="chat-body">
//                                            <strong>Username</strong>
//                                            <small class="pull-right text-muted">
//                                                <i class="fa fa-clock-o fa-fw"></i>28.02.2015 05:15:10
//                                            </small>
//                                            <p>
//                                                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur bibendum ornare dolor, quis ullamcorper ligula sodales.
//                                            </p>
//                                        </div>
//                                    </li>



//<li class="right clearfix">
//                                       <span class="chat-img pull-right">

//                                           <img src="img/2.png" alt="User" class="img-circle" />
//                                       </span>
//                                       <div class="chat-body clearfix">

//                                           <small class=" text-muted">
//                                               <i class="fa fa-clock-o fa-fw"></i>28.02.2015 05:15:10
//                                           </small>
//                                           <strong class="pull-right">Username</strong>
//                                           <p>
//                                               Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur bibendum ornare dolor, quis ullamcorper ligula sodales.
//                                           </p>
//                                       </div>
//                                   </li>