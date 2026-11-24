// Rhothor EasyLaser - Frontend Application Controller
// Zero-dependency HTML5 Canvas rendering for 2D Galvo laser marking

const FIELD_SIZE = 201.65; // mm (matching RTSN-AR180 scanner)
let currentJob = null;
let viewTransform = { x: 0, y: 0, scale: 2.5 }; // canvas pixels per mm
let isDragging = false;
let dragStart = { x: 0, y: 0 };
let currentRotation = 0;

const canvas = document.getElementById('laserCanvas');
const ctx = canvas.getContext('2d');
const container = document.getElementById('canvasContainer');

function resizeCanvas() {
  canvas.width = container.clientWidth;
  canvas.height = container.clientHeight;
  if (viewTransform.x === 0 && viewTransform.y === 0) {
    viewTransform.x = canvas.width / 2;
    viewTransform.y = canvas.height / 2;
  }
  render();
}

window.addEventListener('resize', resizeCanvas);

// Canvas Navigation (Pan & Zoom)
container.addEventListener('mousedown', (e) => {
  if (e.button === 0) { // left button
    isDragging = true;
    dragStart = { x: e.clientX - viewTransform.x, y: e.clientY - viewTransform.y };
  }
});

window.addEventListener('mousemove', (e) => {
  if (isDragging) {
    viewTransform.x = e.clientX - dragStart.x;
    viewTransform.y = e.clientY - dragStart.y;
    render();
  }
  // Update HUD coordinates
  const rect = canvas.getBoundingClientRect();
  const mouseX = e.clientX - rect.left;
  const mouseY = e.clientY - rect.top;
  const mmX = (mouseX - viewTransform.x) / viewTransform.scale;
  const mmY = -(mouseY - viewTransform.y) / viewTransform.scale; // invert Y for Cartesian
  document.getElementById('hudCoords').innerText = `X: ${mmX.toFixed(2)} mm | Y: ${mmY.toFixed(2)} mm`;
});

window.addEventListener('mouseup', () => { isDragging = false; });

container.addEventListener('wheel', (e) => {
  e.preventDefault();
  const zoomFactor = e.deltaY < 0 ? 1.15 : 0.85;
  const rect = canvas.getBoundingClientRect();
  const mouseX = e.clientX - rect.left;
  const mouseY = e.clientY - rect.top;

  viewTransform.x = mouseX - (mouseX - viewTransform.x) * zoomFactor;
  viewTransform.y = mouseY - (mouseY - viewTransform.y) * zoomFactor;
  viewTransform.scale *= zoomFactor;
  render();
});

function zoomIn() {
  viewTransform.scale *= 1.25;
  render();
}

function zoomOut() {
  viewTransform.scale *= 0.8;
  render();
}

function fitView() {
  viewTransform.x = canvas.width / 2;
  viewTransform.y = canvas.height / 2;
  const minDim = Math.min(canvas.width, canvas.height);
  viewTransform.scale = (minDim * 0.85) / FIELD_SIZE;
  render();
}

// Coordinate conversions: mm to canvas pixels
function mmToPx(mmX, mmY) {
  return {
    x: viewTransform.x + mmX * viewTransform.scale,
    y: viewTransform.y - mmY * viewTransform.scale // +Y is up in laser coordinates
  };
}

// Rendering Engine
function render() {
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  // 1. Draw Scanner Working Area Border
  const halfField = FIELD_SIZE / 2;
  const topLeft = mmToPx(-halfField, halfField);
  const sizePx = FIELD_SIZE * viewTransform.scale;

  ctx.strokeStyle = '#252a38';
  ctx.lineWidth = 1;
  ctx.strokeRect(topLeft.x, topLeft.y, sizePx, sizePx);

  // 2. Grid lines
  ctx.strokeStyle = '#1b1e28';
  ctx.lineWidth = 0.5;
  const gridStep = 20; // 20mm grid
  for (let x = -halfField; x <= halfField; x += gridStep) {
    const p1 = mmToPx(x, -halfField);
    const p2 = mmToPx(x, halfField);
    ctx.beginPath();
    ctx.moveTo(p1.x, p1.y);
    ctx.lineTo(p2.x, p2.y);
    ctx.stroke();
  }
  for (let y = -halfField; y <= halfField; y += gridStep) {
    const p1 = mmToPx(-halfField, y);
    const p2 = mmToPx(halfField, y);
    ctx.beginPath();
    ctx.moveTo(p1.x, p1.y);
    ctx.lineTo(p2.x, p2.y);
    ctx.stroke();
  }

  // 3. Center Crosshair (0,0)
  const origin = mmToPx(0, 0);
  ctx.strokeStyle = '#445069';
  ctx.lineWidth = 1.5;
  ctx.beginPath();
  ctx.moveTo(origin.x - 15, origin.y);
  ctx.lineTo(origin.x + 15, origin.y);
  ctx.moveTo(origin.x, origin.y - 15);
  ctx.lineTo(origin.x, origin.y + 15);
  ctx.stroke();

  // 4. Render Artwork Strokes
  if (currentJob && currentJob.strokes) {
    const scale = parseFloat(document.getElementById('inputScale').value) || 1.0;

    // Draw Bounding Box
    if (currentJob.bbox) {
      const b = currentJob.bbox;
      const bMin = mmToPx(b.min_x * scale, b.max_y * scale);
      const bW = (b.max_x - b.min_x) * scale * viewTransform.scale;
      const bH = (b.max_y - b.min_y) * scale * viewTransform.scale;
      ctx.strokeStyle = '#ffea00';
      ctx.setLineDash([4, 4]);
      ctx.lineWidth = 1;
      ctx.strokeRect(bMin.x, bMin.y, bW, bH);
      ctx.setLineDash([]);
    }

    // Draw Vector Strokes
    currentJob.strokes.forEach(stroke => {
      let lastPx = null;
      stroke.forEach(pt => {
        const p = mmToPx(pt.x * scale, pt.y * scale);
        if (pt.type === 'JUMP') {
          if (lastPx) {
            ctx.strokeStyle = '#00d2ff';
            ctx.setLineDash([2, 4]);
            ctx.lineWidth = 1;
            ctx.beginPath();
            ctx.moveTo(lastPx.x, lastPx.y);
            ctx.lineTo(p.x, p.y);
            ctx.stroke();
            ctx.setLineDash([]);
          }
        } else if (pt.type === 'LINE') {
          if (lastPx) {
            ctx.strokeStyle = '#ff3344';
            ctx.lineWidth = 1.8;
            ctx.beginPath();
            ctx.moveTo(lastPx.x, lastPx.y);
            ctx.lineTo(p.x, p.y);
            ctx.stroke();
          }
        }
        lastPx = p;
      });
    });
  }
}

// File Drag & Drop Handling
const dropZone = document.getElementById('dropZone');
const fileInput = document.getElementById('fileInput');

['dragenter', 'dragover'].forEach(eventName => {
  dropZone.addEventListener(eventName, (e) => {
    e.preventDefault();
    dropZone.classList.add('dragover');
  });
});

['dragleave', 'drop'].forEach(eventName => {
  dropZone.addEventListener(eventName, (e) => {
    e.preventDefault();
    dropZone.classList.remove('dragover');
  });
});

dropZone.addEventListener('drop', (e) => {
  const dt = e.dataTransfer;
  const files = dt.files;
  if (files.length > 0) handleFile(files[0]);
});

fileInput.addEventListener('change', (e) => {
  if (fileInput.files.length > 0) handleFile(fileInput.files[0]);
});

function handleFile(file) {
  const reader = new FileReader();
  reader.onload = function(e) {
    const content = e.target.result;
    parseArtwork(content, file.name);
  };
  reader.readAsText(file);
}

function parseArtwork(content, filename) {
  fetch('/api/parse', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ content: content, filename: filename })
  })
  .then(res => res.json())
  .then(data => {
    if (data.status === 'ok') {
      currentJob = data.job;
      updateUIWithJob(currentJob);
      fitView();
    } else {
      alert('Lỗi đọc file: ' + data.message);
    }
  })
  .catch(err => {
    alert('Không thể kết nối với server EasyLaser: ' + err);
  });
}

function updateUIWithJob(job) {
  document.getElementById('fileBadge').innerText = job.filename;
  document.getElementById('fileBadge').classList.add('active');
  document.getElementById('fileInfoBox').style.display = 'block';

  document.getElementById('valFormat').innerText = job.format || 'DXF';
  if (job.dimensions) {
    document.getElementById('valSize').innerText = `${job.dimensions.width_mm.toFixed(1)} x ${job.dimensions.height_mm.toFixed(1)} mm`;
  }
  document.getElementById('valStrokes').innerText = job.strokes ? job.strokes.length : 0;
  document.getElementById('valDistance').innerText = `${(job.total_distance_mm || 0).toFixed(1)} mm`;
  render();
}

// Centering & Transformation
function centerArtwork() {
  if (!currentJob || !currentJob.bbox) return;
  const b = currentJob.bbox;
  const cx = (b.min_x + b.max_x) / 2;
  const cy = (b.min_y + b.max_y) / 2;

  currentJob.strokes.forEach(stroke => {
    stroke.forEach(pt => {
      pt.x -= cx;
      pt.y -= cy;
    });
  });

  currentJob.bbox.min_x -= cx;
  currentJob.bbox.max_x -= cx;
  currentJob.bbox.min_y -= cy;
  currentJob.bbox.max_y -= cy;
  render();
}

function rotateArtwork(deg) {
  if (!currentJob) return;
  const rad = (deg * Math.PI) / 180;
  const cos = Math.cos(rad);
  const sin = Math.sin(rad);

  currentJob.strokes.forEach(stroke => {
    stroke.forEach(pt => {
      const rx = pt.x * cos - pt.y * sin;
      const ry = pt.x * sin + pt.y * cos;
      pt.x = rx;
      pt.y = ry;
    });
  });

  // Recompute bbox
  let minX = 1e9, maxX = -1e9, minY = 1e9, maxY = -1e9;
  currentJob.strokes.forEach(stroke => {
    stroke.forEach(pt => {
      if (pt.x < minX) minX = pt.x;
      if (pt.x > maxX) maxX = pt.x;
      if (pt.y < minY) minY = pt.y;
      if (pt.y > maxY) maxY = pt.y;
    });
  });
  currentJob.bbox = { min_x: minX, max_x: maxX, min_y: minY, max_y: maxY };
  render();
}

// Sliders Event Handlers
document.getElementById('sliderSpeed').addEventListener('input', (e) => {
  document.getElementById('dispSpeed').innerText = `${e.target.value} mm/s`;
});

document.getElementById('sliderPower').addEventListener('input', (e) => {
  document.getElementById('dispPower').innerText = `${e.target.value}%`;
});

document.getElementById('inputScale').addEventListener('input', () => { render(); });

// Operational API calls
function traceFrame() {
  if (!currentJob) {
    alert('Vui lòng mở file DXF/PLT hoặc nạp mẫu thử trước!');
    return;
  }
  const scale = parseFloat(document.getElementById('inputScale').value) || 1.0;
  fetch('/api/frame', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ bbox: currentJob.bbox, scale: scale })
  })
  .then(res => res.json())
  .then(data => {
    if (data.status === 'ok') {
      console.log('Quét khung định vị hoàn thành.');
    } else {
      alert('Lỗi quét khung: ' + data.message);
    }
  });
}

function startMarking() {
  if (!currentJob) {
    alert('Vui lòng mở file DXF/PLT hoặc nạp mẫu thử trước!');
    return;
  }
  const speed = parseFloat(document.getElementById('sliderSpeed').value) || 1500.0;
  const passes = parseInt(document.getElementById('inputPasses').value) || 1;
  const scale = parseFloat(document.getElementById('inputScale').value) || 1.0;

  const btn = document.getElementById('btnMark');
  btn.innerText = '⏳ ĐANG KHẮC LASER...';
  btn.style.background = '#e65100';

  fetch('/api/mark', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      mark_speed: speed,
      jump_speed: 5000.0,
      passes: passes,
      scale: scale,
      strokes: currentJob.strokes
    })
  })
  .then(res => res.json())
  .then(data => {
    btn.innerText = '⚡ 2. BẮT ĐẦU KHẮC LASER (START MARK)';
    btn.style.background = '#00c853';
    if (data.status === 'ok') {
      alert(`Khắc hoàn thành (${passes} lượt)!\nThời gian ước tính: ${data.metrics.estimated_time_seconds || 'N/A'} giây`);
    } else {
      alert('Lỗi khi khắc: ' + data.message);
    }
  })
  .catch(err => {
    btn.innerText = '⚡ 2. BẮT ĐẦU KHẮC LASER (START MARK)';
    btn.style.background = '#00c853';
    alert('Lỗi kết nối: ' + err);
  });
}

function emergencyStop() {
  fetch('/api/abort', { method: 'POST' })
  .then(res => res.json())
  .then(() => {
    alert('ĐÃ DỪNG KHẨN CẤP HỆ THỐNG LASER!');
  });
}

function loadSample(type) {
  if (type === 'dxf') {
    const sampleDxf = `0\nSECTION\n2\nENTITIES\n0\nLINE\n10\n-25.0\n20\n-25.0\n11\n25.0\n21\n-25.0\n0\nLINE\n10\n25.0\n20\n-25.0\n11\n25.0\n21\n25.0\n0\nLINE\n10\n25.0\n20\n25.0\n11\n-25.0\n21\n25.0\n0\nLINE\n10\n-25.0\n20\n25.0\n11\n-25.0\n21\n-25.0\n0\nCIRCLE\n10\n0.0\n20\n0.0\n40\n15.0\n0\nENDSEC\n0\nEOF`;
    parseArtwork(sampleDxf, 'sample_box_circle.dxf');
  } else if (type === 'plt') {
    const samplePlt = `IN;SP1;PU-1000,-1000;PD1000,-1000,1000,1000,-1000,1000,-1000,-1000;PU0,-600;PD600,0,0,600,-600,0,0,-600;PU;`;
    parseArtwork(samplePlt, 'sample_diamond.plt');
  }
}

// Initial status check
fetch('/api/status')
  .then(res => res.json())
  .then(data => {
    const pill = document.getElementById('statusPill');
    if (data.status === 'ok') {
      const dev = data.device;
      pill.innerText = `KẾT NỐI: ${dev.name} #${dev.serial} (${dev.mode})`;
      pill.className = 'status-pill online';
    } else {
      pill.innerText = 'CHẾ ĐỘ MÔ PHỎNG (SIMULATOR)';
      pill.className = 'status-pill online';
    }
  })
  .catch(() => {
    const pill = document.getElementById('statusPill');
    pill.innerText = 'OFFLINE (CHƯA BẬT SERVER)';
    pill.className = 'status-pill offline';
  });

// Initial boot
resizeCanvas();
fitView();
