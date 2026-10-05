# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

<!-- rivet:changelog:start -->

### Added

- Publish release notes with ZIP and Workshop links. ([fa7705f](https://gitlab.com/sgkens/helm-core/-/commit/fa7705ff77f3a24844b60aff3361db25c1641171) — @sgkens)
- Extend display controls, toolbox editing, and release publishing. ([f1c2021](https://gitlab.com/sgkens/helm-core/-/commit/f1c2021610d63f1eabf686f4e2972e0189d37d38) — @sgkens)

### Changed

- Update test suite. ([f2121d4](https://gitlab.com/sgkens/helm-core/-/commit/f2121d45ae741f207a1ab7cc9f9bebb004c1cb83) — @sgkens)
- Update workshop description template. ([09eec87](https://gitlab.com/sgkens/helm-core/-/commit/09eec8704ca26cc6312d8c489601c19288b4edd5) — @sgkens)
- Update gitignore. ([9001e81](https://gitlab.com/sgkens/helm-core/-/commit/9001e81b86c2396ad49e9ebcc73bad3126701d77) — @sgkens)

### Fixed

- Handle plain Workshop description headings. ([e328d6c](https://gitlab.com/sgkens/helm-core/-/commit/e328d6c4d8ccad2365e419da3b71c0299eb2b860) — @sgkens)

<!-- rivet:changelog:end -->

## [0.40.1]

> 2026-10-06

### Added

- Animated liquid power presets for reactors, batteries, solar panels and hydrogen engines.
- Guidance for vertical gauges and panel spans on small displays.
- WASM and browser regression checks for animation, alerts, component edits and corner text sizing.

### Changed

- Add a pastel orange Toolbox launch button beside `./helmv.ps1` in the docs header.
- Reduce corner text scale from `0.55` to `0.40` on 512×128 surfaces and `0.30` on 256×128 surfaces in HELM Core and Toolbox.
- Explain that Play advances animation time while sliders control telemetry; show a reminder when Alert is paused.

### Fixed

- Frozen machinery spinners in Toolbox.
- WASM alert phases drifting from the requested preview time.
- Stale value controls after bind metadata changes.
- Animation stopping after a preview drawing error.
- Missing vendor source maps.
- Hydrogen-engine percentage using enabled status instead of output/max output.
- Overlapping text on dense corner LCD dashboards.
- Duplicate documentation header badges beneath breadcrumbs.
- Invalid Workshop simulator link markup.

## [0.39.0-release]

> 2026-09-29

- No release-note commits were found in the selected range.

## [0.39.0]

> 2026-09-29

### Added

- Add strict semantic version calculator. ([27295b0](https://gitlab.com/sgkens/helm-core/-/commit/27295b0d8c20c323e61c55e5d3013deeb60c8480) — @sgkens)
- Add visual layout inspector. ([f922382](https://gitlab.com/sgkens/helm-core/-/commit/f92238288d2b9e017bcf0b08e553075a49b14cd7) — @sgkens)
- Add project import export and sharing. ([d3e1d33](https://gitlab.com/sgkens/helm-core/-/commit/d3e1d3340392e53e86bb676fcb1436832d96690b) — @sgkens)
- Add attributed block icon references. ([1fe1487](https://gitlab.com/sgkens/helm-core/-/commit/1fe1487854bfdf38604e4aede9f02b9c6f81e53c) — @sgkens)
- Add contextual help and diagnostics. ([c5214f5](https://gitlab.com/sgkens/helm-core/-/commit/c5214f5f456706d3ea65a5d8fc8934a6bf223a82) — @sgkens)
- Build and publish webutil alongside the docs site. ([39b1d42](https://gitlab.com/sgkens/helm-core/-/commit/39b1d42d3062d3c2d3cd9d541d0ed5e2f003c989) — @sgkens)
- An alpha of the browser layout tool at /webutil. ([12fe677](https://gitlab.com/sgkens/helm-core/-/commit/12fe67783059f50cc8e3bcffc263892ed05b07f9) — @sgkens)
- Webutil-wasm reports binds, surface and texture size. ([5ac9a68](https://gitlab.com/sgkens/helm-core/-/commit/5ac9a68f8e019045ecba9160b22f58a0985a8bc0) — @sgkens)

### Changed

- Correted unclosed tag in readme header. ([edb0780](https://gitlab.com/sgkens/helm-core/-/commit/edb078074cf50827ab0056165a87586b65a82738) — @sgkens)
- Update gitignore. ([eac5733](https://gitlab.com/sgkens/helm-core/-/commit/eac5733fb7bbdbd39d0afa4950bf3cc4c0a6642b) — @sgkens)
- Remove compile minfied and combined .cs. ([af4801c](https://gitlab.com/sgkens/helm-core/-/commit/af4801c02617a9e8d7ec2856235717808c5639b5) — @sgkens)
- Add steamcmd uncompleted bash script to tools. ([db99635](https://gitlab.com/sgkens/helm-core/-/commit/db99635ac9fd5d0d6124ac47604dcd24b05bd13e) — @sgkens)
- Update gitignore. ([327fd2e](https://gitlab.com/sgkens/helm-core/-/commit/327fd2e02356fa5151bb3c53ef4b71c50d2b59c7) — @sgkens)
- Update version file. ([410bf27](https://gitlab.com/sgkens/helm-core/-/commit/410bf27c9bd1b2ea58aa31bd5ca049fb5e21e70b) — @sgkens)
- Audited and un-needed development temp docs. ([cd66e3c](https://gitlab.com/sgkens/helm-core/-/commit/cd66e3c25760b0ff265c5a8b6fb951ee2db089dc) — @sgkens)
- Update readme. ([2f4c4b8](https://gitlab.com/sgkens/helm-core/-/commit/2f4c4b8e7d42a2bf683ae29d6e8cc805e4b9d0d1) — @sgkens)
- Update license. ([81302bf](https://gitlab.com/sgkens/helm-core/-/commit/81302bf30994cafacf6a0c68ae34a45bf8920aa0) — @sgkens)
- Add publishing and localization foundations. ([8b3360a](https://gitlab.com/sgkens/helm-core/-/commit/8b3360ac3a26a090317055b679d86f304a720cab) — @sgkens)
- Rename webutil to helm toolbox. ([087fd2b](https://gitlab.com/sgkens/helm-core/-/commit/087fd2b5a1e438c4c791dfe1e21c8d1d99e41922) — @sgkens)
- Add webutil usage guide. ([e59b595](https://gitlab.com/sgkens/helm-core/-/commit/e59b595aea4e3ff49a4dc5d66903a9c5d52407f7) — @sgkens)
- Finish splash and navigation polish. ([054b263](https://gitlab.com/sgkens/helm-core/-/commit/054b263a1cf3c7ad27995da831a0d0fd1ac9c2ed) — @sgkens)
- Drop the numbered '1.', '2.' prefixes from headings. ([2e98639](https://gitlab.com/sgkens/helm-core/-/commit/2e986396146a533da9bcc4908dea0afd09b9ef38) — @sgkens)
- Link to webutil from getting started. ([4f39beb](https://gitlab.com/sgkens/helm-core/-/commit/4f39beb1f7aefa13bbf50d9bc48bc04227aecfde) — @sgkens)
- Extract the sprite-to-SVG renderer into sprite-svg.mjs. ([0187b9e](https://gitlab.com/sgkens/helm-core/-/commit/0187b9ee93c2dbde6f03324a5632d1935c557c79) — @sgkens)

### Fixed

- Make display profiles responsive. ([c3eacc2](https://gitlab.com/sgkens/helm-core/-/commit/c3eacc2f3223b5f1ec3a3607489d27ee54609e83) — @sgkens)
  eeee

## [0.38.3]

> 2026-09-22

### Added

- Names argument and share:kind:Subtype item field. ([9367922](https://gitlab.com/sgkens/helm-core/-/commit/9367922d4fdabc5ff167e8594e48bcc49ca59650) — @sgkens)
- Notes for the dial, bar dressing, segments, rate fields and layout options. ([1c57fb5](https://gitlab.com/sgkens/helm-core/-/commit/1c57fb537aa3f19aae6ac1ee58d1b59e7d19e917) — @sgkens)
- Gap, padding, value(off), icon and blink. ([36d1760](https://gitlab.com/sgkens/helm-core/-/commit/36d1760a42f605f98f1934a57a8dc51398848e3c) — @sgkens)
- Gauge_lab preset. ([bb9bfdf](https://gitlab.com/sgkens/helm-core/-/commit/bb9bfdf69bc3c66060d0cda86b2004867ddb8c3f) — @sgkens)
- Rate, eta and trend fields, and the dial gauge. ([a7880c3](https://gitlab.com/sgkens/helm-core/-/commit/a7880c313cb0a42a4a5e6f0667453895af9f2959) — @sgkens)
- Bar direction, ticks, markers, outline, segment and point counts. ([ab4eca1](https://gitlab.com/sgkens/helm-core/-/commit/ab4eca1e034b5511442d60cc027fe86444db21b4) — @sgkens)
- Draw the border styles in game, for screens and panels. ([61d124c](https://gitlab.com/sgkens/helm-core/-/commit/61d124c7122e966bc4f4f43aa49bba34172602fd) — @sgkens)
- Webutil-wasm spike builds the real script to WebAssembly. ([1b5f905](https://gitlab.com/sgkens/helm-core/-/commit/1b5f905b9d6f610cab7510e9a3f44e777ae9ad31) — @sgkens)
- Color_by, mirror_panel, track for every gauge, gradient answers in backgrounds. ([676b2ff](https://gitlab.com/sgkens/helm-core/-/commit/676b2ffafe3ae048e0aed0c1fb308df6a0495978) — @sgkens)
- Bar_sizes and combo_dashboard presets. ([05cebbb](https://gitlab.com/sgkens/helm-core/-/commit/05cebbb953b896080d0cce38f7479514777fb5b0) — @sgkens)
- Bar sizes, conditional gradients, label and track colours. ([444b7cc](https://gitlab.com/sgkens/helm-core/-/commit/444b7cc4c9ce260cec05f456ccf57162aa6777d9) — @sgkens)
- Show lines of another LCD's text with lineN. ([ec9b7d4](https://gitlab.com/sgkens/helm-core/-/commit/ec9b7d402d9f554ca2635e3f89f6cb945bb2f66d) — @sgkens)
- Mirror another screen with mirror:. ([d57c376](https://gitlab.com/sgkens/helm-core/-/commit/d57c3765ad21fb101d0926c1be6f939db1ada5c6) — @sgkens)
- Liquid tank gauge and vertical panels. ([70800bd](https://gitlab.com/sgkens/helm-core/-/commit/70800bd9724bdfe6d21cfe9fa3dc7ddbdee649e7) — @sgkens)
- Probe test pattern and starter layout for a block with no configuration. ([bfc8cdf](https://gitlab.com/sgkens/helm-core/-/commit/bfc8cdfcd6c724979fa49edfcf0dff25b2f45070) — @sgkens)
- Ranked contents of a cargo group (top1, top2, ...). ([d3b07d3](https://gitlab.com/sgkens/helm-core/-/commit/d3b07d377990eca1422d035054358ab36c95176e) — @sgkens)
- Calibration sweep after the boot. ([2568d44](https://gitlab.com/sgkens/helm-core/-/commit/2568d4418b15059dc0ae23e0161cb5c0e969b883) — @sgkens)
- Smooth gradients for bars and borders. ([3d57bdc](https://gitlab.com/sgkens/helm-core/-/commit/3d57bdcc0c34a8e8f9efd9db7011b3bae64b7e97) — @sgkens)
- Value-driven and flashing backgrounds, panel and horizontal gradients, textures. ([4d5e2ec](https://gitlab.com/sgkens/helm-core/-/commit/4d5e2ec5ec9bede70338bb779c47b7b438b6bbb8) — @sgkens)
- Add projector_welder and rover_suspension presets, document the field scale. ([6bd5fcf](https://gitlab.com/sgkens/helm-core/-/commit/6bd5fcfb472e6d185984764b3de7028e5ec8df4e) — @sgkens)
- Refresh:, alert_period:, scope: all and max(bind.field). ([f0c39f9](https://gitlab.com/sgkens/helm-core/-/commit/f0c39f98094e5a3ee51ce2b9b2a2e0bbc96d45ee) — @sgkens)
- Shorten identifiers in the shipped script and prove it behaves the same. ([fcb97ce](https://gitlab.com/sgkens/helm-core/-/commit/fcb97cecb8be9826b26ff3a05ff88f616114ee6f) — @sgkens)
- Max(N) turns an amount into a percentage for bars and charts. ([82bda96](https://gitlab.com/sgkens/helm-core/-/commit/82bda962c92c2d59084c23d4841175a894d6a21c) — @sgkens)
- Item amounts for cargo and reactor contents. ([83df776](https://gitlab.com/sgkens/helm-core/-/commit/83df7762f8ca75bc8a1d5eb4d0cfb6f10341e0c4) — @sgkens)
- ASCII banners, custom boot logo and boot batches. ([3d59901](https://gitlab.com/sgkens/helm-core/-/commit/3d59901ad8c0d1580923a88fd4e855fcc4b5a2b3) — @sgkens)
- Console in the info pane and on the Programmable Block screen. ([00eaf35](https://gitlab.com/sgkens/helm-core/-/commit/00eaf3535702d04741c242d003035351371ddea5) — @sgkens)
- Boot sequence and adaptive update rate. ([492c81c](https://gitlab.com/sgkens/helm-core/-/commit/492c81c7e8ceeeb06423e48e66f0d63cf74c6ac3) — @sgkens)
- Text values for dynamic binds. ([74e93d0](https://gitlab.com/sgkens/helm-core/-/commit/74e93d0d442787515e055bdb6560c82cd8344dba) — @sgkens)
- Compile the script against the game's own assemblies. ([180390f](https://gitlab.com/sgkens/helm-core/-/commit/180390f8e9f2ec20b7319709cdc619ef9d55e7cd) — @sgkens)
- Add template_nested preset. ([744952a](https://gitlab.com/sgkens/helm-core/-/commit/744952a9f8db9d1b0f8792d97da19c231fba907e) — @sgkens)
- Allow templates to call other templates. ([7fb48d1](https://gitlab.com/sgkens/helm-core/-/commit/7fb48d1faf066ea3c0dd7c7b3df323c21b220258) — @sgkens)
- Nested templates and text dynamic values (size trial). ([3b97bff](https://gitlab.com/sgkens/helm-core/-/commit/3b97bfffffaa5b39391a2cc442baa9625468255c) — @sgkens)
- Add missing_binds preset and spritedump unset mode. ([abb0b6b](https://gitlab.com/sgkens/helm-core/-/commit/abb0b6bb83beb36d2685a882a02fdfad3a44befe) — @sgkens)
- Add documentation site, brand assets and LCD bezel resources. ([052f57d](https://gitlab.com/sgkens/helm-core/-/commit/052f57da918c2abc02993412d58e5d7efc6790a8) — @sgkens)
- Add default preset library. ([f51b865](https://gitlab.com/sgkens/helm-core/-/commit/f51b865aa4b85ef460f1a07ee96948f8329833e3) — @sgkens)
- Add spritedump frame exporter and release helpers. ([37f6589](https://gitlab.com/sgkens/helm-core/-/commit/37f658937a63d66b6d4ca8791ba84fd0739eec4a) — @sgkens)
- Add HELM parser, renderer and tests. ([f1f36ac](https://gitlab.com/sgkens/helm-core/-/commit/f1f36ac1d80764b64440e04a296444fe7c842d25) — @sgkens)

### Changed

- Bump version to 0.38.3 via ccver.ps1. ([1fe5790](https://gitlab.com/sgkens/helm-core/-/commit/1fe5790a709cc876d5d1fb4b6392e09438acd85d) — @sgkens)
- Keep the in-game error and screenshot reports for reference. ([9930771](https://gitlab.com/sgkens/helm-core/-/commit/9930771dd8a0fd4f1024dd1671779acebc87ccf3) — @sgkens)
- Describe the names argument and the share field. ([8cd4416](https://gitlab.com/sgkens/helm-core/-/commit/8cd4416308213d75eb8390b406ca5598fc3f9482) — @sgkens)
- Check names and the item share. ([bb04120](https://gitlab.com/sgkens/helm-core/-/commit/bb04120ad92b92af7a54a8b63483ac5fb72cfd37) — @sgkens)
- Put the measured in-game run into the Workshop text. ([a095f35](https://gitlab.com/sgkens/helm-core/-/commit/a095f355d62d5a370903a5545782eeb9b654c418) — @sgkens)
- Describe the dial, bar dressing, rate fields, layout options and frames. ([6d03a15](https://gitlab.com/sgkens/helm-core/-/commit/6d03a152286540dd836a5c21f36535239608de0d) — @sgkens)
- Check gap, padding, value(off), icon and blink. ([4347588](https://gitlab.com/sgkens/helm-core/-/commit/4347588ba2a783c9f84b309082e1f3f1edb12ef3) — @sgkens)
- Check rate, eta, trend and the dial. ([fdfc583](https://gitlab.com/sgkens/helm-core/-/commit/fdfc583c7e2ee9beaeb8603956919b2b01f92b91) — @sgkens)
- Check bar dressing, segment and point counts. ([b053785](https://gitlab.com/sgkens/helm-core/-/commit/b053785124bff502f040d3ba84ec7cac8d23ac82) — @sgkens)
- Describe the in-game border styles and panel frames. ([0d7cfd9](https://gitlab.com/sgkens/helm-core/-/commit/0d7cfd93899820c92e28bdd0b109627426a342e5) — @sgkens)
- Check double and rounded frames and the panel frame. ([8a4daff](https://gitlab.com/sgkens/helm-core/-/commit/8a4daff54d64938826d696824a410d6e8325a442) — @sgkens)
- Bring the Workshop description up to date. ([fa7ccc9](https://gitlab.com/sgkens/helm-core/-/commit/fa7ccc9b88cb7bd5191bf477a587c1863d1aa37d) — @sgkens)
- Correct the docs-frames comment; note the image builds the WebAssembly spike. ([2fcaa09](https://gitlab.com/sgkens/helm-core/-/commit/2fcaa09b136f5450c2454f00cf5bcc56c90d807b) — @sgkens)
- Correct the helmv parameter table and describe its notes. ([3ab4a72](https://gitlab.com/sgkens/helm-core/-/commit/3ab4a72d81ccf8ea6c242a8db6a49d0813d5a3ef) — @sgkens)
- Document tanks, sizes, colour options, mirrors, LCD text and item fields. ([4a12bbe](https://gitlab.com/sgkens/helm-core/-/commit/4a12bbe32fbd9c3231f40ffcb82541403b87adb3) — @sgkens)
- Check color_by, mirror_panel and track on LED segments. ([19b6098](https://gitlab.com/sgkens/helm-core/-/commit/19b60983ad9911a7c3c1fb4a6064c98f4461d29a) — @sgkens)
- Describe bar sizes, conditional gradients and colour options. ([fd0e912](https://gitlab.com/sgkens/helm-core/-/commit/fd0e912a70b0f1737498660abbead200ef8eb35a) — @sgkens)
- Check bar sizes, conditional gradients, label and track colours. ([c718f36](https://gitlab.com/sgkens/helm-core/-/commit/c718f368af7573348b12e10f458b331faffa1868) — @sgkens)
- Add GetText to the stand-in text surfaces. ([7dc7e66](https://gitlab.com/sgkens/helm-core/-/commit/7dc7e66bdf99512f4f9e9b2ce08c63241260388d) — @sgkens)
- Describe the colour order of a line. ([7fa819b](https://gitlab.com/sgkens/helm-core/-/commit/7fa819b1186cb2021cc96eb82e13e1aadbf15909) — @sgkens)
- Check the flat colour of text and color rules on a bar. ([808cda2](https://gitlab.com/sgkens/helm-core/-/commit/808cda2a613e4c3a96531e495b74c4e6fcbbca53) — @sgkens)
- Describe text from another LCD. ([a02a83a](https://gitlab.com/sgkens/helm-core/-/commit/a02a83a99b2c7d956214ccde3ae1c0dac3e39019) — @sgkens)
- Check note lines and add GetText to the stand-in surfaces. ([3539dc9](https://gitlab.com/sgkens/helm-core/-/commit/3539dc918dbe31c662791ed240022c472ecfdb76) — @sgkens)
- Describe mirror:. ([b11a688](https://gitlab.com/sgkens/helm-core/-/commit/b11a688a997dc893431342a6810d9ec53705712d) — @sgkens)
- Check mirror parsing, scaling and redraw. ([6565e52](https://gitlab.com/sgkens/helm-core/-/commit/6565e52ef3257b714bbe6d6c4cffb9364f9bc01d) — @sgkens)
- Describe tanks and vertical panels. ([2ca5454](https://gitlab.com/sgkens/helm-core/-/commit/2ca545475e8383bb45912b8a664fb31c9293e74c) — @sgkens)
- Check tanks, the liquid gradient and vertical bars. ([849ce7e](https://gitlab.com/sgkens/helm-core/-/commit/849ce7eadee675eca6685f20bfd6acb5dcdc9980) — @sgkens)
- Check sprite alignment, the first frame and the block screen background. ([f1bbc64](https://gitlab.com/sgkens/helm-core/-/commit/f1bbc64d25ca583275e7c3697e6c075e7e7adca1) — @sgkens)
- Describe probe and starter, and the blank-screen cases. ([3b53164](https://gitlab.com/sgkens/helm-core/-/commit/3b53164bec003999b6ac58c14e00f55672e1844d) — @sgkens)
- Check the probe pattern and the starter layout. ([98f6fe8](https://gitlab.com/sgkens/helm-core/-/commit/98f6fe87b5de9b60f06436843da3a527ff7e4095) — @sgkens)
- Explain where bg: applies, screen_bg and that there is no default card. ([51eae4d](https://gitlab.com/sgkens/helm-core/-/commit/51eae4d1bfda87c59aa6caf5caf3386bcd3291fe) — @sgkens)
- A panel_bg card is drawn and the default draws none. ([6e7a43f](https://gitlab.com/sgkens/helm-core/-/commit/6e7a43f7cac283b07261c65c5abd86204ccf984a) — @sgkens)
- Record the approved feature batch and its cost in the efficiency audit. ([b2946d0](https://gitlab.com/sgkens/helm-core/-/commit/b2946d077629e18b86a2bf087d237b2741a16f0e) — @sgkens)
- Describe the ranked contents fields. ([0cb8d17](https://gitlab.com/sgkens/helm-core/-/commit/0cb8d177c29bb212b2b8fa7699f18b0795e92f81) — @sgkens)
- Rank items summed over two inventories. ([80d26db](https://gitlab.com/sgkens/helm-core/-/commit/80d26db2b64876c278d94e5caacd66e14d389371) — @sgkens)
- The game script now runs the calibration sweep. ([b9ba7a8](https://gitlab.com/sgkens/helm-core/-/commit/b9ba7a8c71a48ee8f2c474d566e6ae8b57694fce) — @sgkens)
- Walk the calibration sweep. ([47f6eef](https://gitlab.com/sgkens/helm-core/-/commit/47f6eeffc66c9504e8f82af01e57c19fc2284549) — @sgkens)
- Describe gradient_mode and the blended border gradient. ([e79072f](https://gitlab.com/sgkens/helm-core/-/commit/e79072f30ee5473de80b5551e6589c7a0d87153d) — @sgkens)
- Check smooth and stepped gradients and the blended border. ([985b5e1](https://gitlab.com/sgkens/helm-core/-/commit/985b5e13018760e76ab25a63c26ba56f9ac5acfc) — @sgkens)
- Describe value-driven backgrounds, flash, gradients and textures. ([6811ad9](https://gitlab.com/sgkens/helm-core/-/commit/6811ad92cf4bb8e337f9bdf465d04802634606b6) — @sgkens)
- Check value-driven backgrounds, flash, horizontal gradients and textures. ([6852f0f](https://gitlab.com/sgkens/helm-core/-/commit/6852f0f076c91e4303e478823481a485773eda5b) — @sgkens)
- Read a connector and a door through the block path and draw them. ([1617150](https://gitlab.com/sgkens/helm-core/-/commit/16171501aae89a96e37c058024622cf5204730ee) — @sgkens)
- Describe refresh, alert_period, scope all and max(bind.field). ([1610bb2](https://gitlab.com/sgkens/helm-core/-/commit/1610bb2752045e84e22690d7fcdaf73bf939646d) — @sgkens)
- Check refresh throttling, scope all and a maximum read from another bind. ([aec09d2](https://gitlab.com/sgkens/helm-core/-/commit/aec09d282bf5b2574542a0aa934595d20c8efca0) — @sgkens)
- Describe backgrounds, panel cards and the alert background. ([d7f3b42](https://gitlab.com/sgkens/helm-core/-/commit/d7f3b4264be737e00924bf938e96de262e24eabc) — @sgkens)
- Check gradient bands, the default card, panel_bg none and the alert flash. ([ea16e67](https://gitlab.com/sgkens/helm-core/-/commit/ea16e676701f41f67afd4ba54ab1a4d40ff10354) — @sgkens)
- Record the headroom work in the efficiency audit. ([2bd8f23](https://gitlab.com/sgkens/helm-core/-/commit/2bd8f236e12dbb66df03c86b8a4140d1acd9a87a) — @sgkens)
- Gate on rename parity and check the pre-rename script. ([e72b700](https://gitlab.com/sgkens/helm-core/-/commit/e72b700e68e326ee848e1dc1b47403460b32a682) — @sgkens)
- Describe max(N). ([599f41a](https://gitlab.com/sgkens/helm-core/-/commit/599f41af939d49f789f9065a872971b09742a8ec) — @sgkens)
- Check that max(N) fills a bar to amount over N and caps at full. ([c6235cd](https://gitlab.com/sgkens/helm-core/-/commit/c6235cd70c24862a681f356f540a99ba470a785d) — @sgkens)
- Describe item amounts. ([876a8a9](https://gitlab.com/sgkens/helm-core/-/commit/876a8a980d154330a55978e90d0e3cbfcd2d13e2) — @sgkens)
- Check item amounts across inventories, groups and docked ships. ([4253f72](https://gitlab.com/sgkens/helm-core/-/commit/4253f72832c4664c1a5281a7bbee9d35bb6f754b) — @sgkens)
- Describe banners, boot_logo and boot_batch. ([3da7ba1](https://gitlab.com/sgkens/helm-core/-/commit/3da7ba188d19617277a666b94dcb33e5457fc06e) — @sgkens)
- Drop tool-only code and the redundant private keyword from the script. ([4f85d5b](https://gitlab.com/sgkens/helm-core/-/commit/4f85d5bd7d0421f8840bb10592746361e1b0d45b) — @sgkens)
- Share sprite updates and the percent lookup, keep harness code out of the script. ([0fc4b61](https://gitlab.com/sgkens/helm-core/-/commit/0fc4b61b5aa2f0096bbaa21b0f5fe627f847cecb) — @sgkens)
- Describe the console and the console argument. ([e8a1fc7](https://gitlab.com/sgkens/helm-core/-/commit/e8a1fc76021c409dc1f140051abf3b6a636aff02) — @sgkens)
- Check the console content and that it fits the block's screen. ([d3a1373](https://gitlab.com/sgkens/helm-core/-/commit/d3a13737deb877a81652955ecf57efd3a45744e1) — @sgkens)
- Describe how names match and which blocks HELM uses. ([28cec0a](https://gitlab.com/sgkens/helm-core/-/commit/28cec0ab3d5596c21eb0298bfae00e4f35bed3a4) — @sgkens)
- Check that Program.cs ignores docked ships. ([772367f](https://gitlab.com/sgkens/helm-core/-/commit/772367f524542b453079cc9c75689e8cf29e1d09) — @sgkens)
- Record boot sequence and refresh rate in the efficiency audit. ([edeb44e](https://gitlab.com/sgkens/helm-core/-/commit/edeb44edc8236470b962a7df5aa75030918d36f6) — @sgkens)
- Describe the boot sequence, the boot command and the update rate. ([7359b79](https://gitlab.com/sgkens/helm-core/-/commit/7359b7993cb8090141be15c630f8a4954b247596) — @sgkens)
- Export boot frames and check them in CI. ([2325b74](https://gitlab.com/sgkens/helm-core/-/commit/2325b744c1d4fe8f04677f65b68d34c6010045ef) — @sgkens)
- Explain that HELM is configured from the Programmable Block. ([848a6bf](https://gitlab.com/sgkens/helm-core/-/commit/848a6bfde72bfd27007957264e6ef184376ed6cd) — @sgkens)
- Paste HelmCombined.min.cs and record in-game run 1. ([af09846](https://gitlab.com/sgkens/helm-core/-/commit/af098463203e62c0f23281af01f3cdaec474fe59) — @sgkens)
- Record efficiency audit remediation status. ([10ec4e2](https://gitlab.com/sgkens/helm-core/-/commit/10ec4e27cfea8bff9a0fa3a29843df551b473e62) — @sgkens)
- Verify the minified script and fail on a stale committed build. ([42825a9](https://gitlab.com/sgkens/helm-core/-/commit/42825a90b361855902a6a6fb10c7d8a63f1cf415) — @sgkens)
- Collapse whitespace in the minified script and verify it. ([51768ab](https://gitlab.com/sgkens/helm-core/-/commit/51768ab6184d01f81a8dde6cde9a0f009788895d) — @sgkens)
- Cheaper block fallback, cached colours, kept config errors. ([d4b77fd](https://gitlab.com/sgkens/helm-core/-/commit/d4b77fd1b84cce42f3ba0ffadacd6b67941e4dba) — @sgkens)
- Add efficiency audit. ([c7c04ec](https://gitlab.com/sgkens/helm-core/-/commit/c7c04ecaf832523414d45a1e241d0b48708f0768) — @sgkens)
- Move script size soft limit to 99,500. ([705d8f2](https://gitlab.com/sgkens/helm-core/-/commit/705d8f24b7d427544cae74508e89d06328149eb6) — @sgkens)
- Run every job on phellams-automator 2.22.0. ([465f43d](https://gitlab.com/sgkens/helm-core/-/commit/465f43d6026a65bf721124cd56b7ed27006718a2) — @sgkens)
- Default to the phellams-automator image. ([f0a9b2e](https://gitlab.com/sgkens/helm-core/-/commit/f0a9b2eec2b1db4a0601c5b6a293a2a87bf36b9e) — @sgkens)
- Fail spritedump on empty screens and gate on renderer-smoke. ([dce6b9d](https://gitlab.com/sgkens/helm-core/-/commit/dce6b9dfade7f52b606a5c0cea6c3b6e9296b0fc) — @sgkens)
- Raise script size soft limit to 99,000. ([c42c25d](https://gitlab.com/sgkens/helm-core/-/commit/c42c25db97cd728cda986fea5ec58cc0df720d64) — @sgkens)
- Correct README and workshop page, add repository audit. ([e5f6452](https://gitlab.com/sgkens/helm-core/-/commit/e5f6452acd0bcfdca08880acf4086b9e8b2462da) — @sgkens)
- Add release scripts, combined scripts and CI. ([b5cabb3](https://gitlab.com/sgkens/helm-core/-/commit/b5cabb38f295a6064dba05f3c3021d70add65b74) — @sgkens)
- Add gitignore and version file. ([c1854ec](https://gitlab.com/sgkens/helm-core/-/commit/c1854ecfc0b0f615c345f32934dc46d8402d5040) — @sgkens)

### Fixed

- Walk commits oldest-first in ccver.ps1. ([1e4a9cb](https://gitlab.com/sgkens/helm-core/-/commit/1e4a9cb044286b52cacf2d929847c67347ece050) — @sgkens)
- Restore the notes, the gauge_lab preset, the CI comment and the wasm README. ([0c0c28f](https://gitlab.com/sgkens/helm-core/-/commit/0c0c28f7ba930033ea0dd27010c5bb23e90dcc7a) — @sgkens)
- Restore the border, gauge and helmv pages and the frame check. ([607bc98](https://gitlab.com/sgkens/helm-core/-/commit/607bc982cad59fca1ff5eca132dadafee60bb6ec) — @sgkens)
- Restore the parser modifiers and tests that a stale commit reverted. ([d8d86c7](https://gitlab.com/sgkens/helm-core/-/commit/d8d86c70ded8965a8ea46c249ada8d2fd06b0246) — @sgkens)
- Regenerate stale preview frames before serving. ([a2de9aa](https://gitlab.com/sgkens/helm-core/-/commit/a2de9aae501973b9fa8e5b237fd117ac2933df25) — @sgkens)
- -ScreenName, printed warnings and notes on what the preview cannot draw; preset names read from layouts/. ([13af01b](https://gitlab.com/sgkens/helm-core/-/commit/13af01bdb70364385ddd3b1d2ce2a1a9c5379e12) — @sgkens)
- Every line kind takes color rules, gradient or flat colour. ([6f40871](https://gitlab.com/sgkens/helm-core/-/commit/6f40871f9737ea720eab6a62fe4da8cda0fc2ed3) — @sgkens)
- Centre every texture sprite; draw a rebuilt screen once; black background on the block's screen. ([4159946](https://gitlab.com/sgkens/helm-core/-/commit/4159946065ed91f690f636b12425378e362a97b3) — @sgkens)
- One uniform background by default; warn when only some panels have bg:; add screen_bg:. ([e41c7af](https://gitlab.com/sgkens/helm-core/-/commit/e41c7afdceed697c1e80a494e6342251c23d0442) — @sgkens)
- Helmv compiles HelmCore with HELM_TOOLS again, and draws backgrounds. ([8a340b0](https://gitlab.com/sgkens/helm-core/-/commit/8a340b0bb3d7856b2a980f97023c2822f04c3b7a) — @sgkens)
- Read state and ratio fields on the 0 to 100 scale; add projector, welder and suspension fields. ([047d8a3](https://gitlab.com/sgkens/helm-core/-/commit/047d8a3ce8757a00a6f512386894b27b27180e15) — @sgkens)
- Stop painting a second colour behind panels; add gradient and alert backgrounds. ([26f6a04](https://gitlab.com/sgkens/helm-core/-/commit/26f6a046dbbf9b133d936168c835613c8635e37f) — @sgkens)
- Use only blocks on this construct, never a docked ship's. ([1af7400](https://gitlab.com/sgkens/helm-core/-/commit/1af7400ab8482aefcaf8325fb8f02ded0f14cb34) — @sgkens)
- Use block members that exist in the game API (in-game run 2). ([77ec4aa](https://gitlab.com/sgkens/helm-core/-/commit/77ec4aab29f2fbcff739b4997113649ef571ccec) — @sgkens)
- Emit the paste format the game compiles and check it like the game. ([c9e4414](https://gitlab.com/sgkens/helm-core/-/commit/c9e44143b62ff1d1c20d77a7d99cfd979ba4af67) — @sgkens)
- Compile as C# 6 and avoid prohibited APIs (in-game run 1). ([3826c0f](https://gitlab.com/sgkens/helm-core/-/commit/3826c0f61d6b3383fb377518fe71ebb6b9f2c329) — @sgkens)
- Keep the spritedump marker out of preset descriptions. ([7a22ec8](https://gitlab.com/sgkens/helm-core/-/commit/7a22ec845a0efd7e70d5034b0f3d0c37ef8e5386) — @sgkens)
- Draw the error state when a bind resolves to zero blocks. ([46fac54](https://gitlab.com/sgkens/helm-core/-/commit/46fac5434c695530e99c9ab9f3ea34b52f3db285) — @sgkens)
- Draw bar-only screens on the first frame. ([4935317](https://gitlab.com/sgkens/helm-core/-/commit/4935317c030cab2113f605d78785199ecb189925) — @sgkens)
- Require a Workshop preview and drop the missing logo reference. ([b5e715a](https://gitlab.com/sgkens/helm-core/-/commit/b5e715a127138e18a73b3585209daff55ac9c2eb) — @sgkens)
- Remove links to repository files. ([4dc793a](https://gitlab.com/sgkens/helm-core/-/commit/4dc793ae3f94e0cfc1bace982f90ac7a9c3d8390) — @sgkens)
