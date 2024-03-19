var user = {
    init: function () {
        user.zoomEvent();
    },
    zoomEvent: function () {

        /*!
         @* @name        easyzoom
         @* @author      Matt Hinchliffe
         @* @modified    Friday, December 30th, 2022
         @* @version     2.5.2
         */
        function (t, e) { "use strict"; "function" == typeof define && define.amd ? define(["jquery"], function (t) { e(t) }) : "object" == typeof module && module.exports ? module.exports = t.EasyZoom = e(require("jquery")) : t.EasyZoom = e(t.jQuery) }(this, function (i) { "use strict"; var c, d, l, p, o, s, h = { loadingNotice: "Loading image", errorNotice: "The image could not be loaded", errorDuration: 2500, linkAttribute: "href", preventClicks: !0, beforeShow: i.noop, beforeHide: i.noop, onShow: i.noop, onHide: i.noop, onMove: i.noop }; class n {
            constructor(t, e) { this.$target = i(t), this.opts = i.extend({}, h, e, this.$target.data()), void 0 === this.isOpen && this._init(); }
            _init() { this.$link = this.$target.find("a"), this.$image = this.$target.find("img"), this.$flyout = i('<div class="easyzoom-flyout" />'), this.$notice = i('<div class="easyzoom-notice" />'), this.$target.on({ "mousemove.easyzoom touchmove.easyzoom": i.proxy(this._onMove, this), "mouseleave.easyzoom touchend.easyzoom": i.proxy(this._onLeave, this), "mouseenter.easyzoom touchstart.easyzoom": i.proxy(this._onEnter, this) }), this.opts.preventClicks && this.$target.on("click.easyzoom", function(t) { t.preventDefault(); }); }
            show(t, e) {
                var o = this; if (!1 !== this.opts.beforeShow.call(this)) {
                    if (!this.isReady)
                        return this._loadImage(this.$link.attr(this.opts.linkAttribute), function() { !o.isMouseOver && e || o.show(t); }); this.$target.append(this.$flyout); var i = this.$target.outerWidth(), s = this.$target.outerHeight(), h = this.$flyout.width(), n = this.$flyout.height(), a = this.$zoom.width(), r = this.$zoom.height(); c = Math.ceil(a - h), d = Math.ceil(r - n), l = (c = c < 0 ? 0 : c) / i, p = (d = d < 0 ? 0 : d) / s, this.isOpen = !0, this.opts.onShow.call(this), t && this._move(t);
                }
            }
            _onEnter(t) { var e = t.originalEvent.touches; this.isMouseOver = !0, e && 1 != e.length || (t.preventDefault(), this.show(t, !0)); }
            _onMove(t) { this.isOpen && (t.preventDefault(), this._move(t)); }
            _onLeave() { this.isMouseOver = !1, this.isOpen && this.hide(); }
            _onLoad(t) { t.currentTarget.width && (this.isReady = !0, this.$notice.detach(), this.$flyout.html(this.$zoom), this.$target.removeClass("is-loading").addClass("is-ready"), t.data.call && t.data()); }
            _onError() { var t = this; this.$notice.text(this.opts.errorNotice), this.$target.removeClass("is-loading").addClass("is-error"), this.detachNotice = setTimeout(function() { t.$notice.detach(), t.detachNotice = null; }, this.opts.errorDuration); }
            _loadImage(t, e) { var o = new Image; this.$target.addClass("is-loading").append(this.$notice.text(this.opts.loadingNotice)), this.$zoom = i(o).on("error", i.proxy(this._onError, this)).on("load", e, i.proxy(this._onLoad, this)), o.style.position = "absolute", o.src = t; }
            _move(t) { s = 0 === t.type.indexOf("touch") ? (e = t.touches || t.originalEvent.touches, o = e[0].pageX, e[0].pageY) : (o = t.pageX || o, t.pageY || s); var e = this.$target.offset(), t = o - e.left, e = s - e.top, t = Math.ceil(t * l), e = Math.ceil(e * p); t < 0 || e < 0 || c < t || d < e ? this.hide() : (e = -1 * e, t = -1 * t, "transform" in document.body.style ? this.$zoom.css({ transform: "translate(" + t + "px, " + e + "px)" }) : this.$zoom.css({ top: e, left: t }), this.opts.onMove.call(this, e, t)); }
            hide() { this.isOpen && !1 !== this.opts.beforeHide.call(this) && (this.$flyout.detach(), this.isOpen = !1, this.opts.onHide.call(this)); }
            swap(t, e, o) { this.hide(), this.isReady = !1, this.detachNotice && clearTimeout(this.detachNotice), this.$notice.parent().length && this.$notice.detach(), this.$target.removeClass("is-loading is-ready is-error"), this.$image.attr({ src: t, srcset: i.isArray(o) ? o.join() : o }), this.$link.attr(this.opts.linkAttribute, e); }
            teardown() { this.hide(), this.$target.off(".easyzoom").removeClass("is-loading is-ready is-error"), this.detachNotice && clearTimeout(this.detachNotice), delete this.$link, delete this.$zoom, delete this.$image, delete this.$notice, delete this.$flyout, delete this.isOpen, delete this.isReady; }
        } return,,,,,,,,,,,, i.fn.easyZoom = function (e) { return this.each(function () { var t = i.data(this, "easyZoom"); t ? void 0 === t.isOpen && t._init() : i.data(this, "easyZoom", new n(this, e)) }) }, n });


        // Instantiate EasyZoom instances
        var $easyzoom = $('.easyzoom').easyZoom();

        // Setup thumbnails example
        var api1 = $easyzoom.filter('.easyzoom--with-thumbnails').data('easyZoom');

        $('.thumbnails').on('click', 'a', function (e) {
            var $this = $(this);

            e.preventDefault();

            // Use EasyZoom's `swap` method
            api1.swap($this.data('standard'), $this.attr('href'));
        });

        // Setup toggles example
        var api2 = $easyzoom.filter('.easyzoom--with-toggle').data('easyZoom');

        $('.toggle').on('click', function () {
            var $this = $(this);

            if ($this.data("active") === true) {
                $this.text("Switch on").data("active", false);
                api2.teardown();
            } else {
                $this.text("Switch off").data("active", true);
                api2._init();
            }
        });
    }
}
user.init();