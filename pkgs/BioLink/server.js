import express from 'express';
import fs from 'fs';
import path from 'path';
import crypto from 'crypto';

const app = express();
const PORT = process.env.PORT || 6969;
const WEB_ROOT = process.env.WEB_ROOT || '/var/lib/biolink';

const IP_HASH_SALT = process.env.IP_HASH_SALT || 'change-this-salt-now';

function hashIp(ip) {
  return crypto
    .createHash('sha256')
    .update(ip + IP_HASH_SALT)
    .digest('hex');
}

const webroot = path.join(WEB_ROOT, 'public');
app.use(express.static(webroot));

const dbPath = path.join(WEB_ROOT, 'views.json');

if (!fs.existsSync(dbPath)) {
  fs.writeFileSync(
    dbPath,
    JSON.stringify({ views: 0, ips: [] }, null, 2)
  );
}

function readDB() {
  return JSON.parse(fs.readFileSync(dbPath, 'utf8'));
}

function writeDB(data) {
  fs.writeFileSync(dbPath, JSON.stringify(data, null, 2));
}

function incrementViews(ip) {
  const data = readDB();
  const ipHash = hashIp(ip);

  const alreadySeen = data.ips.includes(ipHash);

  if (!alreadySeen) {
    data.views++;
    data.ips.push(ipHash);
    writeDB(data);
  }

  return data.views;
}

function getClientIp(req) {
  const cf = req.get('cf-connecting-ip');
  if (cf) return cf.trim();

  const real = req.get('x-real-ip');
  if (real) return real.trim();

  return (
    req.socket?.remoteAddress ||
    req.connection?.remoteAddress ||
    ''
  );
}

app.get('/api/views', (req, res) => {
  const ip = getClientIp(req);
  const updated = incrementViews(ip);
  res.json({ views: updated });
});

app.listen(PORT, () =>
  console.log(`Server running on port ${PORT}`)
);